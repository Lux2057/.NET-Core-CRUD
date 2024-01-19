namespace Samples.UploadBigFile.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;
using Samples.UploadBigFile.Shared;

#endregion

public class CreateOrUpdateToDoListWf : HttpBase
{
    #region Constructors

    public CreateOrUpdateToDoListWf(HttpClient http) : base(http) { }

    #endregion

    #region Nested Classes

    public record InitAction(ToDoListDto dto, Action callback = null);

    public record SuccessAction(ToDoListDto dto, Action callback);

    #endregion

    static PaginatedResponseDto<ToDoListSI> copy(PaginatedResponseDto<ToDoListSI> toDoLists, ToDoListDto dto, bool isUpdating)
    {
        return new PaginatedResponseDto<ToDoListSI>
               {
                       Items = toDoLists.Items.Select(r =>
                                                      {
                                                          if (r.Id == dto.Id)
                                                          {
                                                              r.IsUpdating = isUpdating;
                                                              r.Name = dto.Name;
                                                          }

                                                          return r;
                                                      }).ToArray(),
                       PagingInfo = toDoLists.PagingInfo
               };
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnInit(ToDoListsState state, InitAction action)
    {
        var isCreating = state.ToDoLists.Items.All(r => r.Id != action.dto.Id);

        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: isCreating,
                                  toDoLists: copy(state.ToDoLists, action.dto, true));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(InitAction action, IDispatcher dispatcher)
    {
        await this.Http.CreateOrUpdateToDoListAsync(action.dto);

        dispatcher.Dispatch(new SuccessAction(action.dto, action.callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnSuccess(ToDoListsState state, SuccessAction action)
    {
        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: false,
                                  toDoLists: copy(state.ToDoLists, action.dto, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleSuccess(SuccessAction action, IDispatcher dispatcher)
    {
        action.callback?.Invoke();

        return Task.CompletedTask;
    }
}