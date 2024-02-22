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
    private IState<LocalizationState> LocalizationState { get; set; }

    string Language { get => LocalizationState.Value.Language; set => SetLanguage(value); }

    #endregion

    void SetLanguage(string language)
    {
        if (LocalizationState.Value.Language == language)
            return;

        Dispatcher.Dispatch(new SetCultureWf.Init(language,
                                                  Callback: () => Dispatcher.Dispatch(new NavigationWf.NavigateTo(NavigationManager.Uri, true))));
    }
}