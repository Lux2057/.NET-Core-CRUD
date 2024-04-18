namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class RefreshTokenCommand : CommandBase, IRefreshRequest
{
    #region Properties

    public int UserId { get; }

    public string RefreshToken { get; }

    public new AuthInfoDto Result { get; set; }

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
    public class Validator : AbstractValidator<RefreshTokenCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.UserId).NotEmpty().WithMessage(Localization.User_id_cant_be_empty)
                                  .MustAsync((userId, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<UserEntity>(userId)))
                                  .WithMessage(Localization.User_id_is_invalid);
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<RefreshTokenCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var user = await Repository.Read(new FindEntityByIntId<UserEntity>(command.UserId)).SingleOrDefaultAsync(cancellationToken);
            if (user == null)
                throw new ValidationException(new[]
                                              {
                                                      new ValidationFailure(nameof(IRefreshRequest.RefreshToken), Localization.Token_is_invalid)
                                              });

            var refreshToken = await Repository.Read(new IsDeletedProp.FindBy.EqualTo<RefreshTokenEntity>(false) &&
                                                     new UserIdProp.FindBy.EqualTo<RefreshTokenEntity>(user.Id) &&
                                                     new RefreshTokenEntity.FindBy.ExpiresAtGreaterOrEqualTo(DateTime.UtcNow))
                                               .OrderByDescending(r => r.ExpiresAt)
                                               .FirstOrDefaultAsync(cancellationToken);

            if (refreshToken == null)
                throw new ValidationException(new[]
                                              {
                                                      new ValidationFailure(nameof(IRefreshRequest.RefreshToken), Localization.Token_is_expired)
                                              });

            var tokenVerification = new PasswordHasher<RefreshTokenEntity>().VerifyHashedPassword(refreshToken, refreshToken.TokenHash, command.RefreshToken);
            if (tokenVerification != PasswordVerificationResult.Success)
                throw new ValidationException(new[]
                                              {
                                                      new ValidationFailure(nameof(IRefreshRequest.RefreshToken), Localization.Token_is_invalid)
                                              });

            refreshToken.IsDeleted = true;
            await Repository.UpdateAsync(refreshToken);

            var createTokenCommand = new CreateRefreshTokenCommand(user);
            await Dispatcher.PushAsync(createTokenCommand);

            command.Result = createTokenCommand.Result;
        }
    }

    #endregion
}