namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

public class SetValidationStateWf
{
    #region Nested Classes

    public record Init(string Key,
                       ValidationFailureResult ValidationFailure,
                       Action Callback = null);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static ValidationState OnInit(ValidationState _, Init action)
    {
        return new ValidationState(action.Key, action.ValidationFailure);
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleInit(Init action, IDispatcher _)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}