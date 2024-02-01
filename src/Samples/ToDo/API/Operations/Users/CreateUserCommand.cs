namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;

#endregion

public class CreateUserCommand : CommandBase
{
    #region Properties

    public UserAuthDto Dto { get; }

    public new int Result { get; set; }

    #endregion

    #region Constructors

    public CreateUserCommand(UserAuthDto dto)
    {
        Dto = dto;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateUserCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.Dto).NotNull();
            When(r => r.Dto != null,
                 () =>
                 {
                     RuleFor(r => r.Dto.Login).NotEmpty().Must(s =>
                                                               {
                                                                   var task = dispatcher.QueryAsync(new IsLoginUniqueQuery(s));
                                                                   task.Wait();

                                                                   return task.Result;
                                                               }).WithMessage("Login is not unique!");

                     RuleFor(r => r.Dto.Password).NotEmpty().MinimumLength(6);
                 });
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CreateUserCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity { Login = command.Dto.Login };
            userEntity.PasswordHash = new PasswordHasher<UserEntity>().HashPassword(userEntity, command.Dto.Password);

            await Repository.CreateAsync(userEntity);

            command.Result = userEntity.Id;
        }
    }

    #endregion
}