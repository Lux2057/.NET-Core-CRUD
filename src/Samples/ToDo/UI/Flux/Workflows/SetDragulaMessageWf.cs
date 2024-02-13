namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

public class SetDragulaMessageWf
{
    #region Nested Classes

    public record InitAction(string Message);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static DragulaTestState OnInit(DragulaTestState state, InitAction action)
    {
        return new DragulaTestState(message: action.Message);
    }
}