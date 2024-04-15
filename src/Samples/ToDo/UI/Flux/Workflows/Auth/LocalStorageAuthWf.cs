namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Microsoft.JSInterop;
using Samples.ToDo.Shared;

#endregion

public class LocalStorageAuthWf
{
    #region Properties

    private readonly IJSRuntime js;

    #endregion

    #region Constructors

    public LocalStorageAuthWf(IJSRuntime js)
    {
        this.js = js;
    }

    #endregion

    #region Nested Classes

    public record Set(AuthInfoDto AuthInfo);

    public record Fetch(Action<AuthInfoDto> Callback);

    public record Update(AuthInfoDto AuthInfo, Action<AuthInfoDto> Callback);

    #endregion

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleSet(Set action, IDispatcher dispatcher)
    {
        await this.js.SetAuthInfoAsync(action.AuthInfo);
    }

    [ReducerMethod,
     UsedImplicitly]
    public static AuthState OnFetch(AuthState state, Fetch action)
    {
        return new AuthState(isLoading: true,
                             authInfo: state.AuthInfo);
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleFetch(Fetch action, IDispatcher dispatcher)
    {
        var authInfo = await this.js.GetAuthInfoAsync();

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