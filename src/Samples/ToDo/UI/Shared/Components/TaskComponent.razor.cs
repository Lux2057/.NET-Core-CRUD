namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;
using ComponentBase = Samples.ToDo.UI.ComponentBase;

#endregion

public partial class TaskComponent : ComponentBase
{
    #region Properties

    [Parameter, EditorRequired]
    public TaskStatus Status { get; set; }

    [Parameter, EditorRequired]
    public TaskStateDto[] Tasks { get; set; }

    #endregion
}