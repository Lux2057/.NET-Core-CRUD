namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Samples.ToDo.Shared.Auth;

#endregion

public class CreateRefreshTokenCommand : CommandBase
{
    #region Properties

    public UserEntity User { get; }

    public new AuthDto.Result Result { get; set; }

    #endregion

    #region Constructors

    public CreateRefreshTokenCommand(UserEntity user)
    {
        User = user;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CreateRefreshTokenCommand>
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

        protected override async Task Execute(CreateRefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var accessToken = await Dispatcher.QueryAsync(new GetAccessTokenQuery(command.User.GetClaims()));
            var refreshToken = await Dispatcher.QueryAsync(new GetRefreshTokenQuery());

            var tokenEntity = new RefreshTokenEntity
                              {
                                      UserId = command.User.Id,
                                      IssuedAt = DateTime.UtcNow,
                                      ExpiresAt = DateTime.UtcNow.AddMinutes(this.jwtAuthSettings.RefreshTokenExpirationInMinutes)
                              };

            tokenEntity.TokenHash = new PasswordHasher<RefreshTokenEntity>().HashPassword(tokenEntity, refreshToken);

            await Repository.CreateAsync(tokenEntity, cancellationToken);

            command.Result = new AuthDto.Result
                             {
                                     Success = true,
                                     AccessToken = accessToken,
                                     RefreshToken = refreshToken,
                                     User = new UserDto
                                            {
                                                    Id = command.User.Id,
                                                    UserName = command.User.UserName,
                                            }
                             };
        }
    }

    #endregion
}