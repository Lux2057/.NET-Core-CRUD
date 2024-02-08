namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using Extensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;
using Samples.ToDo.Shared.Resources;

#endregion

public class SignInCommand : CommandBase
{
    #region Properties

    public string UserName { get; }

    public string Password { get; }

    public new AuthResultDto AuthResultDto { get; set; }

    #endregion

    #region Constructors

    public SignInCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<SignInCommand>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.UserName).NotEmpty();
            RuleFor(r => r.Password).NotEmpty();
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<SignInCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(SignInCommand command, CancellationToken cancellationToken)
        {
            if (command.UserName.IsNullOrWhitespace() || command.Password.IsNullOrWhitespace())
            {
                command.AuthResultDto = new AuthResultDto
                                 {
                                         Success = false,
                                         Message = Localization.Credentials_are_empty
                                 };

                return;
            }

            var user = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<UserEntity>(false) &&
                                             new UserEntity.FindBy.UserNameEqualTo(command.UserName)).SingleOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                command.AuthResultDto = new AuthResultDto
                                 {
                                         Success = false,
                                         Message = Localization.Credentials_are_invalid
                                 };

                return;
            }

            var verificationResult = new PasswordHasher<UserEntity>().VerifyHashedPassword(user, user.PasswordHash, command.Password);
            if (verificationResult != PasswordVerificationResult.Success)
            {
                command.AuthResultDto = new AuthResultDto
                                 {
                                         Success = false,
                                         Message = Localization.Credentials_are_invalid
                                 };

                return;
            }

            var createTokenCommand = new CreateRefreshTokenCommand(user);
            await Dispatcher.PushAsync(createTokenCommand);

            command.AuthResultDto = createTokenCommand.AuthResultDto;
        }
    }

    #endregion
}