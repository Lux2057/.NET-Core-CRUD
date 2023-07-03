namespace Templates.Blazor.EF.UI.Pages;

#region << Using >>

using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Templates.Blazor.EF.Shared;

#endregion

[Route(UiRoutes.ToDoLists)]
[UsedImplicitly]
public partial class ToDoListsPage : PageBase<ToDoListsState>
{
    #region Constants

    private const string newToDoListNameLabelId = "new-todolist-name-label";

    private const string createToDoListModalId = "create-todolist-modal";

    private const string createToDoListLabelId = "create-todolist-label";

    #endregion

    #region Properties

    private ToDoListDto NewToDoList { get; set; } = new() { Id = -1 };

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        GoToPage(1);
    }

    private void GoToPage(int page)
    {
        Dispatcher.Dispatch(new ReadToDoListsWf.InitAction(page));
    }

    void create()
    {
        Dispatcher.Dispatch(new CreateOrUpdateToDoListWf.InitAction(NewToDoList, () => Dispatcher.Dispatch(new ReadToDoListsWf.InitAction(1))));
    }
}