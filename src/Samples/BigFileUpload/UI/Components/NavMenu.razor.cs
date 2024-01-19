﻿namespace Samples.UploadBigFile.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using ComponentBase = Samples.UploadBigFile.UI.ComponentBase;

#endregion

public partial class NavMenu : ComponentBase
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
}