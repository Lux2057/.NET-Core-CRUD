﻿#pragma warning disable CS8618
namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
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
}

public class PageBase<TState> : PageBase
{
    #region Properties

    [Inject]
    IState<TState> state { get; set; }

    protected TState State => state.Value;

    #endregion
}