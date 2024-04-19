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

        GoToPage(1);
    }

    private void GoToPage(int page)
    {
        Dispatcher.Dispatch(new FetchProjectsWf.Init(page));
    }

    private void CreateProject()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: newProject,
                                                             successCallback: async () =>
                                                                              {
                                                                                  await JS.CloseModalAsync(createProjectModalId);

                                                                                  newProject.Name = string.Empty;
                                                                                  newProject.Description = string.Empty;

                                                                                  GoToPage(1);
                                                                              },
                                                             validationKey: createProjectValidationKey));
    }
}