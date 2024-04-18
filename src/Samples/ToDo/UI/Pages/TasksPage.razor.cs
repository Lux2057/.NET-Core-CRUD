namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
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

    private string Title => $"{Localization[nameof(Resource.Project)]} #{ProjectId}";

    private CreateOrUpdateTaskRequest newTask { get; set; } = new() { Status = TaskStatus.ToDo };

    [Parameter, EditorRequired]
    public int ProjectId { get; set; }

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new FetchTasksWf.Init(ProjectId));

        newTask.ProjectId = ProjectId;
    }

    private void createTask()
    {
        Dispatcher.Dispatch(new CreateOrUpdateTaskWf.Init(request: newTask,
                                                          successCallback: async () =>
                                                                           {
                                                                               await JS.CloseModalAsync(createTaskModalId);

                                                                               newTask.Name = string.Empty;
                                                                               newTask.Description = string.Empty;

                                                                               Dispatcher.Dispatch(new FetchTasksWf.Init(ProjectId));
                                                                           },
                                                          validationKey: createTaskValidationKey));
    }
}