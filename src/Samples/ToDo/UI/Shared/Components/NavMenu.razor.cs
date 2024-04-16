namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using ComponentBase = Samples.ToDo.UI.ComponentBase;

#endregion

public partial class NavMenu : ComponentBase
{
    #region Properties

    [Inject]
    private NavigationManager navigationManager { get; set; }

    private string navMenuCssClass => this.collapseNavMenu ? "collapse" : null;

    private bool collapseNavMenu = true;

    #endregion

    protected override void OnInitialized()
    {
        navigationManager.LocationChanged += (_, _) => StateHasChanged();
    }

    private void toggleNavMenu()
    {
        this.collapseNavMenu = !this.collapseNavMenu;
    }

    private void signOut()
    {
        Dispatcher.Dispatch(new SignOutWf.Init(() => Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Auth, ForceLoad: true))));
    }
}