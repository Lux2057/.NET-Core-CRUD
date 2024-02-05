namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class RefreshTokenCommand : CommandBase
{
    #region Properties

    public int UserId { get; }

    public string RefreshToken { get; }

    public new AuthResultDto Result { get; set; }

    #endregion

    #region Constructors

    public RefreshTokenCommand(int userId, string refreshToken)
    {
        UserId = userId;
        RefreshToken = refreshToken;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<RefreshTokenCommand>
    {
        #region Properties

        private readonly JwtAuthSettings jwtAuthSettings;

        #endregion

        #region Constructors

        public Handler(IServiceProvider serviceProvider, JwtAuthSettings jwtAuthSettings) : base(serviceProvider)
        {
            this.jwtAuthSettings = jwtAuthSettings;
        }

        #endregion

        protected override async Task Execute(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await Repository.Read(new FindEntityByIntId<UserEntity>(command.UserId)).SingleOrDefaultAsync(cancellationToken);
            if (user == null)
            {
                command.Result = new AuthResultDto
                                 {
                                         Success = false,
                                         Message = ValidationMessagesConst.Token_is_invalid
                                 };

                return;
            }

            var refreshToken = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<RefreshTokenEntity>(false) &&
                                                     new UserIdProp.FindBy.EqualTo<RefreshTokenEntity>(user.Id) &&
                                                     new RefreshTokenEntity.FindBy.ExpiresAtGreaterOrEqualTo(DateTime.UtcNow))
                                               .OrderByDescending(r => r.ExpiresAt)
                                               .FirstOrDefaultAsync(cancellationToken);

            if (refreshToken == null)
            {
                command.Result = new AuthResultDto
                                 {
                                         Success = false,
                                         Message = ValidationMessagesConst.Token_is_expired
                                 };

                return;
            }

            var tokenVerification = new PasswordHasher<RefreshTokenEntity>().VerifyHashedPassword(refreshToken, refreshToken.TokenHash, command.RefreshToken);
            if (tokenVerification != PasswordVerificationResult.Success)
            {
                command.Result = new AuthResultDto
                                 {
                                         Success = false,
                                         Message = ValidationMessagesConst.Token_is_invalid
                                 };

                return;
            }

            refreshToken.IsDeleted = true;
            await Repository.UpdateAsync(refreshToken);

            var createTokenCommand = new CreateRefreshTokenCommand(user);
            await Dispatcher.PushAsync(createTokenCommand);

            command.Result = createTokenCommand.Result;
        }
    }

    #endregion
}