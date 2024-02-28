namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using ComponentBase = Samples.ToDo.UI.ComponentBase;

#endregion

public partial class LanguageSelectorComponent : ComponentBase
{
    #region Properties

    [Inject]
    private IState<LocalizationState> localizationState { get; set; }

    private string currentLanguage { get => localizationState.Value.Language; set => setCurrentLanguage(value); }

    #endregion

    private void setCurrentLanguage(string language)
    {
        if (localizationState.Value.Language == language)
            return;

        Dispatcher.Dispatch(new SetCultureWf.Init(Language: language,
                                                  Callback: () => Dispatcher.Dispatch(new NavigationWf.NavigateTo(NavigationManager.Uri, true))));
    }
}