namespace Templates.Blazor.EF.UI;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Templates.Blazor.EF.UI.Localization;

#endregion

#pragma warning disable CS8618
public class ComponentBase : Fluxor.Blazor.Web.Components.FluxorComponent
{
    #region Properties

    [Inject]
    protected IDispatcher Dispatcher { get; set; }

    [Inject]
    protected IStringLocalizer<Resource> Localization { get; set; }

    #endregion
}