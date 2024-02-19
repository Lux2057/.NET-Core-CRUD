namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;
using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

[FeatureState]
public class AuthState
{
    #region Properties

    public DateTime? AuthenticatedAt { get; }

    public bool IsLoading { get; }

    public AuthResultDto AuthResult { get; }

    public bool IsAuthenticated => AuthResult != null &&
                                   !AuthResult.AccessToken.IsNullOrWhitespace() &&
                                   !AuthResult.RefreshToken.IsNullOrWhitespace();

    public bool IsExpiring => IsAuthenticated &&
                              AuthenticatedAt != null &&
                              (DateTime.UtcNow - AuthenticatedAt).Value.Minutes >= 2;

    #endregion

    #region Constructors

    [UsedImplicitly]
    AuthState()
    {
        IsLoading = false;
        AuthResult = null;
        AuthenticatedAt = null;
    }

    public AuthState(bool isLoading,
                     AuthResultDto authResult,
                     DateTime? authenticatedAt)
    {
        IsLoading = isLoading;
        AuthResult = authResult;
        AuthenticatedAt = authenticatedAt;
    }

    #endregion
}