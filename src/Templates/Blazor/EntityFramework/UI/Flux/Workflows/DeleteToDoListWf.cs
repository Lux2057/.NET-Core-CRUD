namespace Templates.Blazor.EF.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

public class DeleteToDoListWf : HttpBase
{
    #region Constructors

    public DeleteToDoListWf(HttpClient http) : base(http) { }

    #endregion

    #region Nested Classes

    public record DeleteAction(int Id);

    public record DeletedAction(int Id);

    #endregion

    static PaginatedResponseDto<ToDoListSI> copy(PaginatedResponseDto<ToDoListSI> toDoLists, int id, bool isDeleted)
    {
        return new PaginatedResponseDto<ToDoListSI>
               {
                       Items = isDeleted ?
                                       toDoLists.Items.Where(r => r.Id != id).ToArray() :
                                       toDoLists.Items.Select(r =>
                                                              {
                                                                  if (r.Id == id)
                                                                      r.IsUpdating = true;

                                                                  return r;
                                                              }).ToArray(),
                       PagingInfo = toDoLists.PagingInfo
               };
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnDelete(ToDoListsState state, DeleteAction action)
    {
        return new ToDoListsState(isLoading: state.IsLoading,
                                  toDoLists: copy(state.ToDoLists, action.Id, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleDelete(DeleteAction action, IDispatcher dispatcher)
    {
        await this.Http.DeleteToDoListAsync(action.Id);

        dispatcher.Dispatch(new DeletedAction(action.Id));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnDeleted(ToDoListsState state, DeletedAction action)
    {
        return new ToDoListsState(isLoading: state.IsLoading,
                                  toDoLists: copy(state.ToDoLists, action.Id, true));
    }
}