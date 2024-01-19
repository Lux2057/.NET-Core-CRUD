namespace Samples.UploadBigFile.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;
using ComponentBase = Samples.UploadBigFile.UI.ComponentBase;

#endregion

public partial class ToDoListComponent : ComponentBase
{
    #region Properties

    [Inject]
    private NavigationManager navigation { get; set; }

    [Parameter]
    [EditorRequired]
    public ToDoListSI Model { get; set; }

    private ToDoListSI State { get; set; }

    [Parameter]
    [EditorRequired]
    public Action OnDeletedCallback { get; set; }

    bool IsEditing { get; set; }

    bool IsConfirmingDeleting { get; set; }

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        State = (ToDoListSI)Model.Clone();
    }

    void edit()
    {
        IsEditing = false;
        Dispatcher.Dispatch(new CreateOrUpdateToDoListWf.InitAction(State));
    }

    void delete()
    {
        IsConfirmingDeleting = false;
        Dispatcher.Dispatch(new DeleteToDoListWf.InitAction(Model.Id, OnDeletedCallback));
    }

    void toggleIsEditing()
    {
        IsEditing = !IsEditing;

        if (IsEditing)
            return;

        State = (ToDoListSI)Model.Clone();
        StateHasChanged();
    }

    void toggleIsConfirmingDeleting()
    {
        IsConfirmingDeleting = !IsConfirmingDeleting;
    }

    void openToDoList()
    {
        navigation.NavigateTo(UiRoutes.ToDoListRoute(State.Id));
    }
}