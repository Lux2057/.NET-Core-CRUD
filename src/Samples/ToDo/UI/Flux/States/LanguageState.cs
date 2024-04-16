namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;
using Fluxor;
using JetBrains.Annotations;
using Newtonsoft.Json;

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
        var jsonLanguage = LocalStorage.GetOrDefault(LocalStorage.Key.Language);
        Language = jsonLanguage.IsNullOrWhitespace() ? string.Empty : JsonConvert.DeserializeObject<string>(jsonLanguage);
    }

    public LanguageState(string language, bool isUpdating)
    {
        Language = language;
        IsUpdating = isUpdating;
    }

    #endregion
}