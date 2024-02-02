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

    public string UserName { get; }

    public string Password { get; }

    public new int Result { get; set; }

    #endregion

    #region Constructors

    public CreateUserCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateUserCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.UserName).NotEmpty().MinimumLength(6).MaximumLength(30)
                                    .MustAsync((userName, _) => dispatcher.QueryAsync(new IsUserNameUniqueQuery(userName)))
                                    .WithMessage(ValidationMessagesConst.User_name_is_not_unique);

            RuleFor(r => r.Password).NotEmpty().MinimumLength(6).MaximumLength(30);
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
            var userEntity = new UserEntity { UserName = command.UserName };
            userEntity.PasswordHash = new PasswordHasher<UserEntity>().HashPassword(userEntity, command.Password);

            await Repository.CreateAsync(userEntity);

            command.Result = userEntity.Id;
        }
    }

    #endregion
}