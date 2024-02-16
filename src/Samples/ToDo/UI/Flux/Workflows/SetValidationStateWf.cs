namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

public class SetValidationStateWf
{
    #region Nested Classes

    public record Init(ValidationFailureResult ValidationFailure);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static ValidationState OnInit(ValidationState _, Init action)
    {
        return new ValidationState(action.ValidationFailure);
    }
}