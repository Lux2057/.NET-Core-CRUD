#pragma warning disable CS8618
namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Samples.ToDo.UI.Localization;

#endregion

public class PageBase : Fluxor.Blazor.Web.Components.FluxorComponent
{
    #region Properties

    [Inject]
    protected IDispatcher Dispatcher { get; set; }

    [Inject]
    protected IStringLocalizer<Resource> Localization { get; set; }

    [Inject]
    protected IJSRuntime JS { get; set; }

    [Inject]
    private IState<AuthState> authState { get; set; }

    protected AuthState AuthState => authState?.Value ?? new AuthState(false, null);

    #endregion

    protected override async Task OnInitializedAsync()
    {
        var authRequired = Attribute.GetCustomAttribute(GetType(), typeof(AuthorizeAttribute)) != null;
        if (authRequired)
        {
            if (AuthState.AuthInfo == null)
                Dispatcher.Dispatch(new LocalStorageAuthWf.Fetch(authInfo =>
                                                                 {
                                                                     if (authInfo == null)
                                                                         Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Auth, true));
                                                                 }));
            else if (!AuthState.IsAuthenticated)
                Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.Auth, true));
        }

        await base.OnInitializedAsync();
    }
}

public class PageBase<TState> : PageBase
{
    #region Properties

    [Inject]
    IState<TState> state { get; set; }

    protected TState State => state.Value;

    #endregion
}