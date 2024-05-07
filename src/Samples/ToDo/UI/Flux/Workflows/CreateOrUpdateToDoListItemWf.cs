namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class CreateOrUpdateToDoListItemWf : HttpBase
{
    #region Constructors

    public CreateOrUpdateToDoListItemWf(HttpClient http) : base(http) { }

    #endregion

    #region Nested Classes

    public record InitAction(ToDoListItemDto dto, Action callback = null);

    public record SuccessAction(ToDoListItemDto dto, Action callback);

    #endregion

    static PaginatedResponseDto<ToDoListItemSI> copy(PaginatedResponseDto<ToDoListItemSI> toDoLists, ToDoListItemDto dto, bool isUpdating)
    {
        return new PaginatedResponseDto<ToDoListItemSI>
               {
                       Items = toDoLists.Items.Select(r =>
                                                      {
                                                          if (r.Id == dto.Id)
                                                          {
                                                              r.IsUpdating = isUpdating;
                                                              r.Description = dto.Description;
                                                              r.Status = dto.Status;
                                                          }

                                                          return r;
                                                      }).ToArray(),
                       PagingInfo = toDoLists.PagingInfo
               };
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListState OnInit(ToDoListState state, InitAction action)
    {
        var isCreating = state.Items.Items.All(r => r.Id != action.dto.Id);

        return new ToDoListState(id: state.Id,
                                 isLoading: state.IsLoading,
                                 isCreating: isCreating,
                                 items: copy(state.Items, action.dto, true));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(InitAction action, IDispatcher dispatcher)
    {
        await this.Http.CreateOrUpdateToDoListItemAsync(action.dto);

        dispatcher.Dispatch(new SuccessAction(action.dto, action.callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListState OnSuccess(ToDoListState state, SuccessAction action)
    {
        return new ToDoListState(id: state.Id,
                                 isLoading: state.IsLoading,
                                 isCreating: false,
                                 items: copy(state.Items, action.dto, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleSuccess(SuccessAction action, IDispatcher dispatcher)
    {
        action.callback?.Invoke();

        return Task.CompletedTask;
    }
}