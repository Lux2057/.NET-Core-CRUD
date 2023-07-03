namespace Templates.Blazor.NH.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using ComponentBase = Templates.Blazor.NH.UI.ComponentBase;

#endregion

public partial class LoaderComponent : UI.ComponentBase
{
    #region Properties

    [Parameter]
    public bool IsSmall { get; set; }

    [Parameter]
    public bool IsInline { get; set; }

    private string cssClass { get; set; } = "";

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (IsSmall)
            cssClass = "spinner-border-sm";
    }
}