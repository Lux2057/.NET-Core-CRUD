namespace Templates.Blazor.EF.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Components;
using Templates.Blazor.EF.Shared;

#endregion

[Route(UiRoutes.ToDoList)]
public partial class ToDoListPage : PageBase<ToDoListState>
{
    #region Properties

    [Parameter]
    public int Id { get; set; }

    private ToDoListItemDto NewToDoListItem { get; set; } = new();

    private const string createToDoListItemModalId = "create-item-modal";

    private const string createToDoListItemLabelId = "create-item-modal-label";

    private const string newToDoListItemDescriptionLabelId = "new-item-description-label";

    #endregion

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        NewToDoListItem.ToDoListId = Id;
        NewToDoListItem.Status = ItemStatus.ToDo;

        GoToPage(1);
    }

    private void GoToPage(int page)
    {
        Dispatcher.Dispatch(new ReadToDoListWf.InitAction(Id, page));
    }

    void create()
    {
        Dispatcher.Dispatch(new CreateOrUpdateToDoListItemWf.InitAction(NewToDoListItem, () => Dispatcher.Dispatch(new ReadToDoListWf.InitAction(Id, 1))));
    }
}