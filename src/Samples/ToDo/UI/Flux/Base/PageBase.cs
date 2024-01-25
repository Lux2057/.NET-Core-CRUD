#pragma warning disable CS8618
namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using Samples.ToDo.UI.Localization;

#endregion

public class PageBase<TState> : Fluxor.Blazor.Web.Components.FluxorComponent
{
    #region Properties

    [Inject]
    protected IDispatcher Dispatcher { get; set; }

    [Inject]
    IState<TState> state { get; set; }

    protected TState State => state.Value;

    [Inject]
    protected IStringLocalizer<Resource> Localization { get; set; }

    [Inject]
    protected IJSRuntime JS { get; set; }

    #endregion
}

public class PageBase : Microsoft.AspNetCore.Components.ComponentBase
{
    #region Properties

    [Inject]
    protected IStringLocalizer<Resource> Localization { get; set; }

    [Inject]
    protected IJSRuntime JS { get; set; }

    #endregion
}