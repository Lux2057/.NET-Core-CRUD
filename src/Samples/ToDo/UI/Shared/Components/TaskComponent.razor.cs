namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;
using ComponentBase = Samples.ToDo.UI.ComponentBase;

#endregion

public partial class TaskComponent : ComponentBase
{
    #region Properties

    [Parameter, EditorRequired]
    public TaskStateDto Model { get; set; }

    private string deleteTaskTitle => $"{Localization[Resource.Delete]} {Localization[Resource.Task]} #{Model.Id}";

    private string deleteTaskModalId => $"delete-task-{Model.Id}-modal";

    private string deleteTaskValidationKey => $"delete-task-{Model.Id}-validation";

    #endregion

    private void DeleteTask()
    {
        Dispatcher.Dispatch(new DeleteTaskWf.Init(request: new DeleteEntityRequest { Id = Model.Id },
                                                  callback: async success =>
                                                            {
                                                                if (success)
                                                                    await JS.CloseModalAsync(deleteTaskModalId);
                                                            },
                                                  validationKey: deleteTaskValidationKey));
    }
}