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

    private bool isProjectEditing { get; set; }

    private string deleteProjectTitle => $"{Localization[Resource.Delete]} {Localization[Resource.Project]} #{Model?.Id}";

    private string deleteProjectModalId => $"delete-project-{Model.Id}-modal";

    private string deleteProjectValidationKey => $"delete-project-{Model.Id}-validation";

    private string updateProjectValidationKey => $"update-project-{Model.Id}-modal";

    [Inject]
    private IState<ValidationState> validationState { get; set; }

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (ProjectStatedDto)Model.Clone();
    }

    private void UpdateProject()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: new CreateOrUpdateProjectRequest
                                                                      {
                                                                              Id = State.Id,
                                                                              Description = State.Description,
                                                                              Name = State.Name
                                                                      },
                                                             validationKey: updateProjectValidationKey,
                                                             callback: (success) =>
                                                                       {
                                                                           if (success)
                                                                               isProjectEditing = false;
                                                                           else if (validationState.Value.ValidationErrors(updateProjectValidationKey)?.Any() != true)
                                                                               Dispatcher.Dispatch(new SetValidationStateWf.Init(updateProjectValidationKey,
                                                                                                                                 new ValidationFailureResult(Localization[Resource.Http_request_error],
                                                                                                                                                             Array.Empty<ValidationError>())));

                                                                           InvokeAsync(StateHasChanged);
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

    private void ToggleIsProjectEditing()
    {
        isProjectEditing = !isProjectEditing;

        if (isProjectEditing)
            return;

        State = (ProjectStatedDto)Model.Clone();
        StateHasChanged();
    }

    private void OpenProjectPage()
    {
        Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.TasksRoute(Model.Id)));
    }
}