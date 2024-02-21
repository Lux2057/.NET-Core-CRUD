namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class SignUpWf
{
    #region Properties

    private readonly AuthAPI authApi;

    #endregion

    #region Constructors

    public SignUpWf(HttpClient http, IDispatcher dispatcher)
    {
        this.authApi = new AuthAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init(AuthRequest Request, Action<AuthInfoDto> Callback) : IValidatingAction
    {
        #region Properties

        public string ValidationKey { get; set; }

        #endregion
    }

    public record Update(AuthInfoDto AuthInfo, Action<AuthInfoDto> Callback);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static AuthState OnInit(AuthState state, Init _)
    {
        return new AuthState(isLoading: true,
                             authInfo: state.AuthInfo,
                             authenticatedAt: state.AuthenticatedAt);
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SetValidationStateWf.Init(action.ValidationKey, null));

        var authResult = await this.authApi.SignUpAsync(request: action.Request,
                                                        validationKey: action.ValidationKey);

        dispatcher.Dispatch(new Update(authResult, action.Callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static AuthState OnUpdate(AuthState _, Update action)
    {
        return new AuthState(isLoading: false,
                             authInfo: action.AuthInfo,
                             authenticatedAt: action.AuthInfo != null ? DateTime.UtcNow : null);
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher _)
    {
        action.Callback?.Invoke(action.AuthInfo);

        return Task.CompletedTask;
    }
}