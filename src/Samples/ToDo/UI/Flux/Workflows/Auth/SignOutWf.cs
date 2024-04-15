namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

public class SignOutWf
{
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
    public Task HandleInit(Init action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new Update(action.Callback));

        dispatcher.Dispatch(new LocalStorageAuthWf.Set(null));

        return Task.CompletedTask;
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