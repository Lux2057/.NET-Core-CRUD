namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;
using Fluxor;
using JetBrains.Annotations;
using Newtonsoft.Json;
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
                              (DateTime.UtcNow - AuthInfo.AuthenticatedAt).Minutes >= 2;

    #endregion

    #region Constructors

    [UsedImplicitly]
    AuthState()
    {
        IsLoading = false;

        var authInfoJson = LocalStorage.GetBuiltInValueOrDefault(LocalStorage.Key.AuthInfo);
        AuthInfo = authInfoJson.IsNullOrWhitespace() ? null : JsonConvert.DeserializeObject<AuthInfoDto>(authInfoJson);
    }

    public AuthState(bool isLoading,
                     AuthInfoDto authInfo)
    {
        IsLoading = isLoading;
        AuthInfo = authInfo;
    }

    #endregion
}