namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class RefreshAccessTokenWf
{
    #region Properties

    private readonly AuthAPI authApi;

    #endregion

    #region Constructors

    public RefreshAccessTokenWf(HttpClient http,
                                IDispatcher dispatcher,
                                IState<LocalizationState> localizationState)
    {
        this.authApi = new AuthAPI(http, dispatcher, localizationState);
    }

    #endregion

    #region Nested Classes

    public record Init(string RefreshToken, Action<AuthInfoDto> Callback = default) : IValidatingAction
    {
        #region Properties

        public string ValidationKey { get; set; }

        #endregion
    }

    public record Update(AuthInfoDto AuthInfo, Action<AuthInfoDto> Callback);

    #endregion

    [ReducerMethod,
     UsedImplicitly]
    public static AuthState OnInit(AuthState state, Init action)
    {
        return new AuthState(isLoading: true,
                             authInfo: state.AuthInfo);
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var authInfo = await this.authApi.RefreshTokenAsync(request: new RefreshTokenRequestDto { RefreshToken = action.RefreshToken },
                                                            validationKey: action.ValidationKey);

        dispatcher.Dispatch(new Update(authInfo, action.Callback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static AuthState OnUpdate(AuthState state, Update action)
    {
        return new AuthState(isLoading: false,
                             authInfo: action.AuthInfo);
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke(action.AuthInfo);

        return Task.CompletedTask;
    }
}