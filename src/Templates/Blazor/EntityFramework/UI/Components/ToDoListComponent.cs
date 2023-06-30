namespace Templates.Blazor.EF.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;

#endregion

public partial class ToDoListComponent : UI.ComponentBase
{
    #region Properties

    [Inject]
    private NavigationManager navigation { get; set; }

    [Parameter]
    [EditorRequired]
    public ToDoListSI Model { get; set; }

    [Parameter]
    [EditorRequired]
    public Action OnDeletedCallback { get; set; }

    bool IsEditing { get; set; }

    bool IsConfirmingDeleting { get; set; }

    #endregion

    void edit()
    {
        IsEditing = false;
        Dispatcher.Dispatch(new CreateOrUpdateToDoListWf.InitAction(Model));
    }

    void delete()
    {
        IsConfirmingDeleting = false;
        Dispatcher.Dispatch(new DeleteToDoListWf.InitAction(Model.Id, OnDeletedCallback));
    }

    void toggleIsEditing()
    {
        IsEditing = !IsEditing;
    }

    void toggleIsConfirmingDeleting()
    {
        IsConfirmingDeleting = !IsConfirmingDeleting;
    }
}