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

    bool IsProjectEditing { get; set; }

    private string ValidationKey = Guid.NewGuid().ToString();

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (ProjectStateDto)Model.Clone();
    }

    void UpdateProject()
    {
        IsProjectEditing = false;
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(request: new CreateOrUpdateProjectRequest
                                                                      {
                                                                              Id = State.Id,
                                                                              Description = State.Description,
                                                                              Name = State.Name,
                                                                              TagsIds = State.Tags.Select(r => r.Id).ToArray()
                                                                      },
                                                             validationKey: this.ValidationKey));
    }

    void ToggleIsProjectEditing()
    {
        IsProjectEditing = !IsProjectEditing;

        if (IsProjectEditing)
            return;

        State = (ProjectStateDto)Model.Clone();
        StateHasChanged();
    }

    void OpenProjectPage() { }
}