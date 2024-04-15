namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class SignInWf
{
    #region Properties

    private readonly AuthAPI authApi;

    #endregion

    #region Constructors

    public SignInWf(HttpClient http,
                    IDispatcher dispatcher,
                    IState<LocalizationState> localizationState)
    {
        this.authApi = new AuthAPI(http, dispatcher, localizationState);
    }

    #endregion

    #region Nested Classes

    public record Init : ValidatingAction<AuthRequest>
    {
        #region Properties

        public Action<AuthInfoDto> Callback { get; }

        #endregion

        #region Constructors

        public Init(AuthRequest request,
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
        dispatcher.Dispatch(new SetValidationStateWf.Init(action.ValidationKey, null));

        var authInfo = await this.authApi.SignInAsync(request: action.Request,
                                                      validationKey: action.ValidationKey);

        dispatcher.Dispatch(new LocalStorageAuthWf.Set(authInfo));

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