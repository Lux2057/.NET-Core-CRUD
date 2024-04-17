namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

[Route(UiRoutes.Projects), Authorize]
public partial class ProjectsPage : PageBase<ProjectsPageState>
{
    #region Constants

    private const string createProjectModalId = "create-project-modal";

    private const string createProjectValidationKey = "create-project-validation";

    #endregion

    #region Properties

    private CreateOrUpdateProjectRequest newProject { get; set; } = new();

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        goToPage(1);
    }

    private void goToPage(int page)
    {
        Dispatcher.Dispatch(new FetchProjectsWf.Init(page));
    }

    private void createProject()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: newProject,
                                                             successCallback: async () =>
                                                                              {
                                                                                  await JS.CloseModalAsync(createProjectModalId);
                                                                                  newProject = new();
                                                                                  goToPage(1);
                                                                              },
                                                             validationKey: createProjectValidationKey));
    }
}