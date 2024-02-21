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

    public AuthInfoDto AuthInfo { get; }

    public bool IsAuthenticated => AuthInfo != null &&
                                   !AuthInfo.AccessToken.IsNullOrWhitespace() &&
                                   !AuthInfo.RefreshToken.IsNullOrWhitespace();

    public bool IsExpiring => IsAuthenticated &&
                              AuthenticatedAt != null &&
                              (DateTime.UtcNow - AuthenticatedAt).Value.Minutes >= 2;

    #endregion

    #region Constructors

    [UsedImplicitly]
    AuthState()
    {
        IsLoading = false;
        AuthInfo = null;
        AuthenticatedAt = null;
    }

    public AuthState(bool isLoading,
                     AuthInfoDto authInfo,
                     DateTime? authenticatedAt)
    {
        IsLoading = isLoading;
        AuthInfo = authInfo;
        AuthenticatedAt = authenticatedAt;
    }

    #endregion
}