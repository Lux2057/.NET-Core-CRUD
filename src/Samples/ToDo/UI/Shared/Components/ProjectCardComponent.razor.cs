namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

public partial class ProjectCardComponent : UI.ComponentBase
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
        isProjectEditing = false;
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: new CreateOrUpdateProjectRequest
                                                                      {
                                                                              Id = State.Id,
                                                                              Description = State.Description,
                                                                              Name = State.Name,
                                                                              TagsIds = State.Tags.Select(r => r.Id).ToArray()
                                                                      },
                                                             validationKey: this.validationKey));
    }

    private void toggleIsProjectEditing()
    {
        isProjectEditing = !isProjectEditing;

        if (isProjectEditing)
            return;

        State = (ProjectStateDto)Model.Clone();
        StateHasChanged();
    }

    private void openProjectPage() { }
}