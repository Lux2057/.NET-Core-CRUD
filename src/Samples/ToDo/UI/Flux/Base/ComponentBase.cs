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
    IState<AuthState> authState { get; set; }

    protected AuthState AuthState => authState.Value;

    [Inject]
    protected IDispatcher Dispatcher { get; set; }

    [Inject]
    protected IStringLocalizer<Resource> Localization { get; set; }

    #endregion
}