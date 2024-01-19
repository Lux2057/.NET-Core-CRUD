namespace Samples.UploadBigFile.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

public class DeleteToDoListItemWf : HttpBase
{
    #region Constructors

    public DeleteToDoListItemWf(HttpClient http) : base(http) { }

    #endregion

    #region Nested Classes

    public record InitAction(int Id, Action callback);

    public record SuccessAction(int Id, Action callback);

    #endregion

    static PaginatedResponseDto<ToDoListItemSI> copy(PaginatedResponseDto<ToDoListItemSI> toDoLists, int id, bool isDeleted)
    {
        return new PaginatedResponseDto<ToDoListItemSI>
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
    public static ToDoListState OnInit(ToDoListState state, InitAction action)
    {
        return new ToDoListState(id: state.Id,
                                 isLoading: state.IsLoading,
                                 isCreating: state.IsCreating,
                                 items: copy(state.Items, action.Id, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(InitAction action, IDispatcher dispatcher)
    {
        await this.Http.DeleteToDoListItemAsync(action.Id);

        dispatcher.Dispatch(new SuccessAction(action.Id, action.callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListState OnSuccess(ToDoListState state, SuccessAction action)
    {
        return new ToDoListState(id: state.Id,
                                 isLoading: state.IsLoading,
                                 isCreating: state.IsCreating,
                                 items: copy(state.Items, action.Id, true));
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleSuccess(SuccessAction action, IDispatcher dispatcher)
    {
        action.callback?.Invoke();

        return Task.CompletedTask;
    }
}