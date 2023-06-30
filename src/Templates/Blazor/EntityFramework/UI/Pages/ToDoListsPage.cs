﻿namespace Templates.Blazor.EF.UI.Pages;

#region << Using >>

using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

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

    private string NewToDoListName { get; set; }

    #endregion

    protected override void OnInitialized()
    {
        base.OnInitialized();

        GoToPage(1);
    }

    private void GoToPage(int page)
    {
        Dispatcher.Dispatch(new ReadToDoListsWf.FetchPageAction(page));
    }

    void create()
    {
        Dispatcher.Dispatch(new CreateOrUpdateToDoListWf.CreateOrUpdateToDoListAction(-1, NewToDoListName));
    }
}