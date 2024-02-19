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
        RefreshAuth();

        Dispatcher.Dispatch(new FetchProjectsWf.Init(page, AuthState.AuthResult.AccessToken));
    }

    void create()
    {
        RefreshAuth();

        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(Project: NewProject,
                                                             AccessToken: AuthState.AuthResult.AccessToken,
                                                             IsUpdate: false,
                                                             Callback: () => GoToPage(1)));
    }
}