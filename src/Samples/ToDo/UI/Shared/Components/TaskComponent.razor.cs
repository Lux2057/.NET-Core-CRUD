namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;
using ComponentBase = Samples.ToDo.UI.ComponentBase;

#endregion

#endregion

public partial class TaskComponent : ComponentBase
{
    #region Properties

    [Parameter, EditorRequired]
    public TaskStateDto Model { get; set; }

    [Parameter, EditorRequired]
    public int ProjectId { get; set; }

    [Inject]
    private IState<ValidationState> validationState { get; set; }

    private TaskDto State { get; set; }

    private string updateTaskTitle => $"{Localization[Resource.Edit]} {Localization[Resource.Task]} #{Model.Id}";

    private string updateTaskModalId => $"update-task-{Model.Id}-modal";

    private string updateTaskValidationKey => $"update-task-{Model.Id}-validation";

    private string deleteTaskTitle => $"{Localization[Resource.Delete]} {Localization[Resource.Task]} #{Model.Id}";

    private string deleteTaskModalId => $"delete-task-{Model.Id}-modal";

    private string deleteTaskValidationKey => $"delete-task-{Model.Id}-validation";

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (TaskStateDto)Model.Clone();
    }

    private void Update()
    {
        Dispatcher.Dispatch(new CreateOrUpdateTaskWf.Init(request: new CreateOrUpdateTaskRequest
                                                                   {
                                                                           Id = State.Id,
                                                                           Description = State.Description,
                                                                           Name = State.Name,
                                                                           ProjectId = ProjectId
                                                                   },
                                                          validationKey: updateTaskValidationKey,
                                                          callback: async (success) =>
                                                                    {
                                                                        if (success)
                                                                            await JS.CloseModalAsync(updateTaskModalId);
                                                                        else if (validationState.Value.ValidationErrors(updateTaskValidationKey)?.Any() != true)
                                                                            Dispatcher.Dispatch(new SetValidationStateWf.Init(updateTaskValidationKey,
                                                                                                                              new ValidationFailureResult(Localization[Resource.Http_request_error],
                                                                                                                                                          Array.Empty<ValidationError>())));

                                                                        await InvokeAsync(StateHasChanged);
                                                                    }));
    }

    private void Delete()
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