namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Samples.ToDo.Shared;

#endregion

public class SignInWf
{
    #region Properties

    private readonly AuthAPI authApi;

    #endregion

    #region Constructors

    public SignInWf(HttpClient http)
    {
        this.authApi = new AuthAPI(http);
    }

    #endregion

    #region Nested Classes

    [Authorize]
    public record Init(AuthRequest Request, Action<AuthResultDto> Callback) : IValidationAPI
    {
        #region Properties

        public string ValidationKey { get; set; }

        #endregion
    }

    public record Update(AuthResultDto AuthResult, Action<AuthResultDto> Callback);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static AuthState OnInit(AuthState state, Init _)
    {
        return new AuthState(isLoading: true,
                             authResult: state.AuthResult,
                             authenticatedAt: state.AuthenticatedAt);
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SetValidationStateWf.Init(action.ValidationKey, null));

        var authResult = await this.authApi.SignInAsync(request: action.Request,
                                                        validationFailCallback: result => dispatcher.Dispatch(new SetValidationStateWf.Init(action.ValidationKey, result)));

        dispatcher.Dispatch(new Update(authResult, action.Callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static AuthState OnUpdate(AuthState _, Update action)
    {
        return new AuthState(isLoading: false,
                             authResult: action.AuthResult,
                             authenticatedAt: action.AuthResult.Success ? DateTime.UtcNow : null);
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher _)
    {
        action.Callback?.Invoke(action.AuthResult);

        return Task.CompletedTask;
    }
}