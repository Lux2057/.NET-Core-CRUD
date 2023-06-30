namespace Templates.Blazor.EF.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;
using Templates.Blazor.EF.Shared;

#endregion

public class CreateOrUpdateToDoListWf : HttpBase
{
    #region Constructors

    public CreateOrUpdateToDoListWf(HttpClient http) : base(http) { }

    #endregion

    #region Nested Classes

    public record InitAction(int Id, string Name, Action callback);

    public record SuccessAction(int Id, string Name, Action callback);

    #endregion

    static PaginatedResponseDto<ToDoListSI> copy(PaginatedResponseDto<ToDoListSI> toDoLists, int id, string name, bool isUpdating)
    {
        return new PaginatedResponseDto<ToDoListSI>
               {
                       Items = toDoLists.Items.Select(r =>
                                                      {
                                                          if (r.Id == id)
                                                          {
                                                              r.IsUpdating = isUpdating;
                                                              r.Name = name;
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
        var isCreating = state.ToDoLists.Items.All(r => r.Id != action.Id);

        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: isCreating,
                                  toDoLists: copy(state.ToDoLists, action.Id, action.Name, true));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(InitAction action, IDispatcher dispatcher)
    {
        await this.Http.CreateOrUpdateToDoListAsync(new ToDoListDto
                                                    {
                                                            Id = action.Id,
                                                            Name = action.Name
                                                    });

        dispatcher.Dispatch(new SuccessAction(action.Id, action.Name, action.callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnSuccess(ToDoListsState state, SuccessAction action)
    {
        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: false,
                                  toDoLists: copy(state.ToDoLists, action.Id, action.Name, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleSuccess(SuccessAction action, IDispatcher dispatcher)
    {
        action.callback?.Invoke();

        return Task.CompletedTask;
    }
}