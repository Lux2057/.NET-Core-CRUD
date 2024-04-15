namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Microsoft.JSInterop;
using Samples.ToDo.Shared;

#endregion

public class SignOutWf
{
    #region Properties

    private readonly IJSRuntime js;

    #endregion

    #region Constructors

    public SignOutWf(IJSRuntime js)
    {
        this.js = js;
    }

    #endregion

    #region Nested Classes

    public record Init(Action Callback = default);

    public record Update(Action Callback);

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
        dispatcher.Dispatch(new Update(action.Callback));

        await LocalStorage.SetAsync(this.js, LocalStorage.Key.AuthInfo, (AuthInfoDto)null);
    }

    [ReducerMethod,
     UsedImplicitly]
    public static AuthState OnUpdate(AuthState state, Update action)
    {
        return new AuthState(isLoading: false,
                             authInfo: null);
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}