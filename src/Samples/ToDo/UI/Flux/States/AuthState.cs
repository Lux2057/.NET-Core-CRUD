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

    public bool IsLoading { get; }

    public AuthInfoDto AuthInfo { get; }

    public bool IsAuthenticated => AuthInfo != null &&
                                   !AuthInfo.AccessToken.IsNullOrWhitespace() &&
                                   !AuthInfo.RefreshToken.IsNullOrWhitespace();

    public bool IsExpiring => IsAuthenticated &&
                              (DateTime.UtcNow - AuthInfo.AuthenticatedAt).Seconds >= 30;

    #endregion

    #region Constructors

    [UsedImplicitly]
    AuthState()
    {
        IsLoading = false;
        AuthInfo = LocalStorage.GetOrDefault<AuthInfoDto>(LocalStorage.Key.AuthInfo);
    }

    public AuthState(bool isLoading,
                     AuthInfoDto authInfo)
    {
        IsLoading = isLoading;
        AuthInfo = authInfo;
    }

    #endregion
}