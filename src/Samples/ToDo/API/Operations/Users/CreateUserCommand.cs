namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Samples.ToDo.Shared;

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
                     RuleFor(r => r.Dto.UserName).NotEmpty().MinimumLength(6).MaximumLength(30)
                                                 .MustAsync((login, _) => dispatcher.QueryAsync(new IsLoginUniqueQuery(login)))
                                                 .WithMessage(ValidationMessagesConst.Login_is_not_unique);

                     RuleFor(r => r.Dto.Password).NotEmpty().MinimumLength(6).MaximumLength(30);
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
            var userEntity = new UserEntity { UserName = command.Dto.UserName };
            userEntity.PasswordHash = new PasswordHasher<UserEntity>().HashPassword(userEntity, command.Dto.Password);

            await Repository.CreateAsync(userEntity);

            command.Result = userEntity.Id;
        }
    }

    #endregion
}