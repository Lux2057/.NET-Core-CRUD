namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

[Route(UiRoutes.Projects), Authorize]
public partial class ProjectsPage : PageBase<ProjectsState>
{
    #region Constants

    private const string createProjectModalId = "create-project-modal";

    private const string createProjectValidationKey = "create-project-validation";

    #endregion

    #region Properties

    private CreateOrUpdateProjectRequest NewProject { get; set; } = new();

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

    void CreateProject()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(Request: NewProject,
                                                             SuccessCallback: async () =>
                                                                              {
                                                                                  await JS.CloseModalAsync(createProjectModalId);
                                                                                  NewProject = new();
                                                                                  GoToPage(1);
                                                                              })
                            {
                                    ValidationKey = createProjectValidationKey
                            });
    }
}