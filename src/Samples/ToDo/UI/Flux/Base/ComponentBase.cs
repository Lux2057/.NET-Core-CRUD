namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Samples.ToDo.UI.Localization;

#endregion

#pragma warning disable CS8618
public class ComponentBase : Fluxor.Blazor.Web.Components.FluxorComponent
{
    #region Properties

    [Inject]
    protected IDispatcher Dispatcher { get; set; }

    [Inject]
    protected IStringLocalizer<Resource> Localization { get; set; }

    [Inject]
    private IState<AuthState> authState { get; set; }

    protected AuthState AuthState => authState?.Value ?? new AuthState(false, null);

    #endregion
}

public class ComponentBase<TState> : ComponentBase
{
    #region Properties

    [Inject]
    IState<TState> state { get; set; }

    protected TState State => state.Value;

    #endregion
}