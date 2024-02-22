namespace Samples.ToDo.UI;

#region << Using >>

using System.Globalization;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class LocalizationState
{
    #region Properties

    public bool IsUpdating { get; }

    public string Language { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    LocalizationState()
    {
        Language = CultureInfo.CurrentCulture.Name;
    }

    public LocalizationState(string language, bool isUpdating)
    {
        Language = language;
        IsUpdating = isUpdating;
    }

    #endregion
}