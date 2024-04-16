namespace Samples.ToDo.UI;

#region << Using >>

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
        IsUpdating = false;
        Language = LocalStorage.GetOrDefault<string>(LocalStorage.Key.Language);
    }

    public LanguageState(string language, bool isUpdating)
    {
        Language = language;
        IsUpdating = isUpdating;
    }

    #endregion
}