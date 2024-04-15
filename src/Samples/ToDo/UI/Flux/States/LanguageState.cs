namespace Samples.ToDo.UI;

#region << Using >>

using System.Globalization;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class LanguageState
{
    #region Properties

    public bool IsUpdating { get; }

    public string Language { get; }

    #endregion

    #region Constructors

    [UsedImplicitly]
    LanguageState()
    {
        Language = CultureInfo.CurrentCulture.Name;
    }

    public LanguageState(string language, bool isUpdating)
    {
        Language = language;
        IsUpdating = isUpdating;
    }

    #endregion
}