namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;

#endregion

[Route(UiRoutes.Tasks), Authorize]
public partial class TasksPage : PageBase<TasksPageState>
{
    #region Constants

    private const string createTaskModalId = "create-task-modal";

    private const string createTaskValidationKey = "create-task-validation";

    #endregion

    #region Properties

    [Inject]
    private IState<ValidationState> validationState { get; set; }

    private string Title => $"{Localization[nameof(Resource.Project)]} #{ProjectId}";

    private CreateOrUpdateTaskRequest newTask { get; set; } = new() { Status = TaskStatus.ToDo };

    [Parameter, EditorRequired]
    public int ProjectId { get; set; }

    private Dictionary<TaskStatus, TaskStateDto[]> StatusGroups => this.statuses.ToDictionary(status => status, status => State.Tasks.Where(x => x.Status == status).OrderBy(r => r.StatusUpDt).ToArray());

    readonly TaskStatus[] statuses = Enum.GetValues<TaskStatus>().ToArray();

    private DotNetObjectReference<TasksPage> refObj;

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new FetchTasksWf.Init(ProjectId));

        newTask.ProjectId = ProjectId;

        this.refObj = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JS.InitDragulaAsync(this.refObj,
                                      this.statuses.Select(r => r.ToString()).ToArray(),
                                      nameof(DropCallback));

        await base.OnAfterRenderAsync(firstRender);
    }

    protected override void Dispose(bool disposing)
    {
        this.refObj?.Dispose();

        base.Dispose(disposing);
    }

    [JSInvokable]
    public void DropCallback(string uid, string source, string target)
    {
        var id = int.Parse(uid);
        var taskStatus = Enum.Parse<TaskStatus>(target);

        Dispatcher.Dispatch(new SetTaskStatusWf.Init(new SetTaskStatusRequest
                                                     {
                                                             Id = id,
                                                             Status = taskStatus
                                                     },
                                                     failCallback: () => Dispatcher.Dispatch(new NavigationWf.Refresh(true))));
    }

    private void createTask()
    {
        Dispatcher.Dispatch(new CreateOrUpdateTaskWf.Init(request: newTask,
                                                          callback: async (success) =>
                                                                    {
                                                                        if (success)
                                                                            await JS.CloseModalAsync(createTaskModalId);

                                                                        newTask.Name = string.Empty;
                                                                        newTask.Description = string.Empty;

                                                                        if (success)
                                                                            Dispatcher.Dispatch(new FetchTasksWf.Init(ProjectId));
                                                                        else if (validationState.Value.ValidationErrors(createTaskValidationKey)?.Any() != true)
                                                                            Dispatcher.Dispatch(new SetValidationStateWf.Init(createTaskValidationKey,
                                                                                                                              new ValidationFailureResult(Localization[Resource.Http_request_error],
                                                                                                                                                          Array.Empty<ValidationError>())));
                                                                    },
                                                          validationKey: createTaskValidationKey));
    }
}