namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using ComponentBase = Samples.ToDo.UI.ComponentBase;

#endregion

public partial class NavMenu : ComponentBase
{
    #region Properties

    [Inject]
    private IState<AuthState> authState { get; set; }

    private AuthState AuthState => authState.Value;

    private string NavMenuCssClass => this.collapseNavMenu ? "collapse" : null;

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
        Dispatcher.Dispatch(new SignOutWf.Init(() => NavigationManager.NavigateTo(UiRoutes.Auth)));
    }
}