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

    public record CreateOrUpdateToDoListAction(int Id, string Name);

    public record ToDoListCreatedOrUpdatedAction(int Id, string Name, bool isNew);

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
    public static ToDoListsState OnCreateOrUpdate(ToDoListsState state, CreateOrUpdateToDoListAction action)
    {
        var isCreating = state.ToDoLists.Items.All(r => r.Id != action.Id);

        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: isCreating,
                                  toDoLists: copy(state.ToDoLists, action.Id, action.Name, true));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleCreateOrUpdate(CreateOrUpdateToDoListAction action, IDispatcher dispatcher)
    {
        var response = await this.Http.CreateOrUpdateToDoListAsync(new ToDoListDto
                                                                   {
                                                                           Id = action.Id,
                                                                           Name = action.Name
                                                                   });

        var id = int.Parse(await response.Content.ReadAsStringAsync());

        dispatcher.Dispatch(new ToDoListCreatedOrUpdatedAction(action.Id, action.Name, id != action.Id));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnCreatedOrUpdated(ToDoListsState state, ToDoListCreatedOrUpdatedAction action)
    {
        return new ToDoListsState(isLoading: state.IsLoading,
                                  isCreating: false,
                                  toDoLists: copy(state.ToDoLists, action.Id, action.Name, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleCreatedOrUpdated(ToDoListCreatedOrUpdatedAction action, IDispatcher dispatcher)
    {
        if (action.isNew)
            dispatcher.Dispatch(new ReadToDoListsWf.FetchPageAction(1));

        return Task.CompletedTask;
    }
}