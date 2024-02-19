namespace Samples.ToDo.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;

#endregion

public partial class ModalWindowComponent : ComponentBase
{
    #region Properties

    [Parameter]
    public string ModalId { get; set; } = $"modal-{Guid.NewGuid():N}";

    [Parameter]
    public RenderFragment ButtonContent { get; set; }

    [Parameter]
    public RenderFragment Title { get; set; }

    [Parameter]
    public RenderFragment Body { get; set; }

    [Parameter]
    public RenderFragment Footer { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }

    private string ModalLabelId => $"{ModalId}-label";

    #endregion
}