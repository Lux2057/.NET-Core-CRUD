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

    bool IsEditing { get; set; }

    bool IsConfirmingDeleting { get; set; }

    #endregion

    void edit()
    {
        Dispatcher.Dispatch(new CreateOrUpdateToDoListWf.CreateOrUpdateToDoListAction(Model.Id, Model.Name));
    }

    void delete()
    {
        Dispatcher.Dispatch(new DeleteToDoListWf.DeleteAction(Model.Id));
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