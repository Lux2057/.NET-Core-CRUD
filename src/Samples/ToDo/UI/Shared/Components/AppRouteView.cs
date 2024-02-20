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
    NavigationManager NavigationManager { get; set; }

    [Inject]
    IState<AuthState> AuthState { get; set; }

    #endregion

    protected override void Render(RenderTreeBuilder builder)
    {
        var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
        if (authorize && AuthState?.Value?.IsAuthenticated != true)
            NavigationManager.NavigateTo(UiRoutes.Auth);
        else
            base.Render(builder);
    }
}