namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;

#endregion

public partial class ProjectRowComponent : UI.ComponentBase
{
    #region Properties

    [Parameter, EditorRequired]
    public ProjectStatedDto Model { get; set; }

    private ProjectDto State { get; set; }

    private string deleteProjectTitle => $"{Localization[Resource.Delete]} {Localization[Resource.Project]} #{Model?.Id}";

    private string deleteProjectModalId => $"delete-project-{Model.Id}-modal";

    private string deleteProjectValidationKey => $"delete-project-{Model.Id}-validation";

    private string updateProjectModalId => $"update-project-{Model.Id}-modal";

    private string updateProjectValidationKey => $"update-project-{Model.Id}-validation";

    private string updateProjectTitle => $"{Localization[Resource.Edit]} {Localization[Resource.Project]} #{Model.Id}";

    [Inject]
    private IState<ValidationState> validationState { get; set; }

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (ProjectStatedDto)Model.Clone();
    }

    private void Update()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: new CreateOrUpdateProjectRequest
                                                                      {
                                                                              Id = State.Id,
                                                                              Description = State.Description,
                                                                              Name = State.Name
                                                                      },
                                                             validationKey: updateProjectValidationKey,
                                                             callback: async (success) =>
                                                                       {
                                                                           if (success)
                                                                               await JS.CloseModalAsync(updateProjectModalId);
                                                                           else if (validationState.Value.ValidationErrors(updateProjectValidationKey)?.Any() != true)
                                                                               Dispatcher.Dispatch(new SetValidationStateWf.Init(updateProjectValidationKey,
                                                                                                                                 new ValidationFailureResult(Localization[Resource.Http_request_error],
                                                                                                                                                             Array.Empty<ValidationError>())));

                                                                           await InvokeAsync(StateHasChanged);
                                                                       }));
    }

    private void DeleteProject()
    {
        Dispatcher.Dispatch(new DeleteProjectWf.Init(request: new DeleteEntityRequest { Id = Model.Id },
                                                     callback: async success =>
                                                               {
                                                                   if (success)
                                                                       await JS.CloseModalAsync(deleteProjectModalId);
                                                               },
                                                     validationKey: deleteProjectValidationKey));
    }

    private void InitState()
    {
        State = (ProjectStatedDto)Model.Clone();

        StateHasChanged();
    }

    private void OpenProjectPage()
    {
        Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.TasksRoute(Model.Id)));
    }
}