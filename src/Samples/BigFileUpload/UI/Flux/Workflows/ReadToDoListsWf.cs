namespace Samples.UploadBigFile.UI;

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

    public record InitAction(int Page);

    public record SuccessAction(PaginatedResponseDto<ToDoListSI> ToDoLists);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnInit(ToDoListsState state, InitAction action)
    {
        return new ToDoListsState(isLoading: true,
                                  isCreating: state.IsCreating,
                                  toDoLists: state.ToDoLists);
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(InitAction action, IDispatcher dispatcher)
    {
        var pageData = await this.Http.ReadToDoListsAsync<ToDoListSI>(action.Page);

        dispatcher.Dispatch(new SuccessAction(pageData));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListsState OnSuccess(ToDoListsState state, SuccessAction action)
    {
        return new ToDoListsState(isLoading: false,
                                  isCreating: state.IsCreating,
                                  toDoLists: action.ToDoLists);
    }
}