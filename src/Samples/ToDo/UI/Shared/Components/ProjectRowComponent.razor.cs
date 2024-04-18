namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

public partial class ProjectRowComponent : UI.ComponentBase
{
    #region Properties

    [Parameter]
    [EditorRequired]
    public ProjectStateDto Model { get; set; }

    private ProjectDto State { get; set; }

    private bool isProjectEditing { get; set; }

    private string validationKey = Guid.NewGuid().ToString();

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (ProjectStateDto)Model.Clone();
    }

    private void updateProject()
    {
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: new CreateOrUpdateProjectRequest
                                                                      {
                                                                              Id = State.Id,
                                                                              Description = State.Description,
                                                                              Name = State.Name
                                                                      },
                                                             validationKey: this.validationKey,
                                                             successCallback: () =>
                                                                              {
                                                                                  isProjectEditing = false;
                                                                                  InvokeAsync(StateHasChanged);
                                                                              }));
    }

    private void toggleIsProjectEditing()
    {
        isProjectEditing = !isProjectEditing;

        if (isProjectEditing)
            return;

        State = (ProjectStateDto)Model.Clone();
        StateHasChanged();
    }

    private void openProjectPage()
    {
        Dispatcher.Dispatch(new NavigationWf.NavigateTo(UiRoutes.TasksRoute(Model.Id)));
    }
}