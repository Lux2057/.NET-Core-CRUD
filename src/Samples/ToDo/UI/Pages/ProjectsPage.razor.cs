namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Fluxor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;

#endregion

[Route(UiRoutes.Projects), Authorize]
public partial class ProjectsPage : PageBase<ProjectsPageState>
{
    #region Constants

    private const string createProjectModalId = "create-project-modal";

    private const string createProjectValidationKey = "create-project-validation";

    #endregion

    #region Properties

    [Inject]
    private IState<ValidationState> validationState { get; set; }

    private CreateOrUpdateProjectRequest newProject { get; set; } = new();

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        GoToPage(1);
    }

    private void GoToPage(int page)
    {
        Dispatcher.Dispatch(new FetchProjectsWf.Init(page));
    }

    private void CreateProject()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: newProject,
                                                             callback: async (success) =>
                                                                       {
                                                                           if (success)
                                                                               await JS.CloseModalAsync(createProjectModalId);

                                                                           newProject.Name = string.Empty;
                                                                           newProject.Description = string.Empty;

                                                                           if (success)
                                                                               GoToPage(1);
                                                                           else if (validationState.Value.ValidationErrors(createProjectValidationKey)?.Any() != true)
                                                                               Dispatcher.Dispatch(new SetValidationStateWf.Init(createProjectValidationKey,
                                                                                                                                 new ValidationFailureResult(Localization[Resource.Http_request_error],
                                                                                                                                                             Array.Empty<ValidationError>())));
                                                                       },
                                                             validationKey: createProjectValidationKey));
    }
}