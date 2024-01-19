namespace Samples.UploadBigFile.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using ComponentBase = Samples.UploadBigFile.UI.ComponentBase;

#endregion

public partial class ToDoListItemComponent : ComponentBase
{
    #region Properties

    [Parameter]
    [EditorRequired]
    public ToDoListItemSI Model { get; set; }

    private ToDoListItemSI State { get; set; }

    [Parameter]
    [EditorRequired]
    public Action OnDeletedCallback { get; set; }

    bool IsEditing { get; set; }

    bool IsConfirmingDeleting { get; set; }

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (ToDoListItemSI)Model.Clone();
    }

    void edit()
    {
        IsEditing = false;
        Dispatcher.Dispatch(new CreateOrUpdateToDoListItemWf.InitAction(State));
    }

    void delete()
    {
        IsConfirmingDeleting = false;
        Dispatcher.Dispatch(new DeleteToDoListItemWf.InitAction(Model.Id, OnDeletedCallback));
    }

    void toggleIsEditing()
    {
        IsEditing = !IsEditing;

        if (IsEditing)
            return;

        State = (ToDoListItemSI)Model.Clone();
        StateHasChanged();
    }

    void toggleIsConfirmingDeleting()
    {
        IsConfirmingDeleting = !IsConfirmingDeleting;
    }
}