#pragma warning disable CS8618
namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Samples.ToDo.UI.Localization;

#endregion

public class PageBase<TState> : PageBase
{
    #region Properties

    [Inject]
    IState<TState> state { get; set; }

    protected TState State => state.Value;

    #endregion
}

public class PageBase : Fluxor.Blazor.Web.Components.FluxorComponent
{
    #region Properties

    [Inject]
    IState<AuthState> authState { get; set; }

    protected AuthState AuthState => authState.Value;

    [Inject]
    protected IDispatcher Dispatcher { get; set; }

    [Inject]
    protected IStringLocalizer<Resource> Localization { get; set; }

    [Inject]
    protected IJSRuntime JS { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new SetValidationStateWf.Init(null));
    }

    protected void RefreshAuth()
    {
        if (AuthState.IsAuthenticated)
        {
            if (AuthState.IsExpiring)
                Dispatcher.Dispatch(new RefreshAccessTokenWf.Init(AuthState.AuthResult.RefreshToken,
                                                                  authResult =>
                                                                  {
                                                                      if (authResult.Success)
                                                                          return;

                                                                      NavigationManager.NavigateTo(UiRoutes.Auth, true);
                                                                  }));
        }
        else
        {
            NavigationManager.NavigateTo(UiRoutes.Auth, true);
        }
    }
}