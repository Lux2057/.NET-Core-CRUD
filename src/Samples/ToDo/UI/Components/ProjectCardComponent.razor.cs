namespace Samples.ToDo.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Samples.ToDo.Shared;

#endregion

public partial class ProjectCardComponent : UI.ComponentBase
{
    #region Properties

    [Parameter]
    [EditorRequired]
    public ProjectEditableDto Model { get; set; }

    private ProjectDto State { get; set; }

    bool IsEditing { get; set; }

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (ProjectEditableDto)Model.Clone();
    }

    void edit()
    {
        IsEditing = false;
        Dispatcher.Dispatch(new CreateOrUpdateProjectWf.Init(Project: State, IsUpdate: true));
    }

    void toggleIsEditing()
    {
        IsEditing = !IsEditing;

        if (IsEditing)
            return;

        State = (ProjectEditableDto)Model.Clone();
        StateHasChanged();
    }

    void openProjectPage() { }
}