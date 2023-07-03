namespace Templates.Blazor.NH.UI;

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

    public record InitAction(int Id, Action callback);

    public record SuccessAction(int Id, Action callback);

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
    public static ToDoListsState OnInit(ToDoListsState state, InitAction action)
    {
        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: state.IsCreating,
                                  toDoLists: copy(state.ToDoLists, action.Id, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(InitAction action, IDispatcher dispatcher)
    {
        await this.Http.DeleteToDoListAsync(action.Id);

        dispatcher.Dispatch(new SuccessAction(action.Id, action.callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnSuccess(ToDoListsState state, SuccessAction action)
    {
        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: state.IsCreating,
                                  toDoLists: copy(state.ToDoLists, action.Id, true));
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleSuccess(SuccessAction action, IDispatcher dispatcher)
    {
        action.callback?.Invoke();

        return Task.CompletedTask;
    }
}