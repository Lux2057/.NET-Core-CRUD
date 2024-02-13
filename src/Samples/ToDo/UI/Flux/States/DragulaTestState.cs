namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class DragulaTestState
{
    #region Properties

    public string Message { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    DragulaTestState()
    {
        Message = string.Empty;
    }

    public DragulaTestState(string message)
    {
        Message = message;
    }

    #endregion
}