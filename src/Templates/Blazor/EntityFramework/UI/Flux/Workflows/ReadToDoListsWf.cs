namespace Templates.Blazor.EF.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

public class ReadToDoListsWf : HttpBase
{
    #region Constructors

    public ReadToDoListsWf(HttpClient http) : base(http) { }

    #endregion

    #region Nested Classes

    public record FetchPageAction(int Page);

    public record PageFetchedAction(PaginatedResponseDto<ToDoListSI> ToDoLists);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnFetchPage(ToDoListsState state, FetchPageAction action)
    {
        return new ToDoListsState(isLoading: true,
                                  toDoLists: state.ToDoLists);
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleFetchPage(FetchPageAction action, IDispatcher dispatcher)
    {
        var pageData = await this.Http.ReadToDoListsAsync<ToDoListSI>(action.Page);

        dispatcher.Dispatch(new PageFetchedAction(pageData));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnPageFetched(ToDoListsState state, PageFetchedAction action)
    {
        return new ToDoListsState(isLoading: false,
                                  toDoLists: action.ToDoLists);
    }
}