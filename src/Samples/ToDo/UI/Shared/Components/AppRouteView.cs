namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

#endregion

public class AppRouteView : RouteView
{
    #region Properties

    [Inject]
    private NavigationManager navigationManager { get; set; }

    [Inject]
    private IState<AuthState> authState { get; set; }

    #endregion

    protected override void Render(RenderTreeBuilder builder)
    {
        var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
        if (authorize && authState?.Value?.IsAuthenticated != true)
            navigationManager.NavigateTo(UiRoutes.Auth);
        else
            base.Render(builder);
    }
}