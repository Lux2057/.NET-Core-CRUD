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

    private ProjectDto NewProject { get; set; } = new() { Id = -1 };

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

    void create()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(Project: NewProject,
                                                             IsUpdate: false,
                                                             SuccessCallback: async () =>
                                                                       {
                                                                           await JS.CloseModalAsync(createProjectModalId);
                                                                           NewProject = new() { Id = -1 };
                                                                           GoToPage(1);
                                                                       })
                            {
                                    ValidationKey = createProjectValidationKey
                            });
    }
}