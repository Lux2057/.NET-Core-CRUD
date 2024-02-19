namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

public class SignOutWf
{
    #region Nested Classes

    public record Init(Action Callback);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static AuthState OnInit(AuthState state, Init _)
    {
        return new AuthState(isLoading: false,
                             authResult: null,
                             authenticatedAt: null);
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleInit(Init action, IDispatcher _)
    {
        action.Callback?.Invoke();
        return Task.CompletedTask;
    }
}