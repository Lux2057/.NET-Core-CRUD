namespace Samples.ToDo.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;

#endregion

public partial class NavMenu : UI.ComponentBase
{
    #region Properties

    private string NavMenuCssClass => this.collapseNavMenu ? "collapse" : null;

    [Inject]
    NavigationManager NavigationManager { get; set; }

    private bool collapseNavMenu = true;

    #endregion

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += (s, e) => StateHasChanged();
    }

    private void ToggleNavMenu()
    {
        this.collapseNavMenu = !this.collapseNavMenu;
    }

    void SignOut()
    {
        Dispatcher.Dispatch(new AuthWf.SignOutWf.Init(() => NavigationManager.NavigateTo(UiRoutes.Auth)));
    }
}