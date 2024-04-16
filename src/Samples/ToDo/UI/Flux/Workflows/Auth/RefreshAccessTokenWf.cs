namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Microsoft.JSInterop;
using Samples.ToDo.Shared;

#endregion

public class RefreshAccessTokenWf
{
    #region Properties

    private readonly AuthAPI authApi;

    private readonly IJSRuntime js;

    #endregion

    #region Constructors

    public RefreshAccessTokenWf(HttpClient http,
                                IDispatcher dispatcher,
                                IState<LanguageState> localizationState,
                                IJSRuntime js)
    {
        this.authApi = new AuthAPI(http, dispatcher, localizationState);
        this.js = js;
    }

    #endregion

    #region Nested Classes

    public record Init : ValidatingAction<RefreshTokenRequest>
    {
        #region Properties

        public Action<AuthInfoDto> Callback { get; }

        #endregion

        #region Constructors

        public Init(RefreshTokenRequest request,
                    Action<AuthInfoDto> callback = default,
                    string validationKey = default)
                : base(request, validationKey)
        {
            Callback = callback;
        }

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
        var authInfo = await this.authApi.RefreshTokenAsync(request: action.Request, validationKey: action.ValidationKey);

        await this.js.SetLocalStorageAsync(LocalStorage.Key.AuthInfo, authInfo);

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