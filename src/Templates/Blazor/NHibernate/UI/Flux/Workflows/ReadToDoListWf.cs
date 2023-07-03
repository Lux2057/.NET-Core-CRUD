namespace Templates.Blazor.NH.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

public class ReadToDoListWf : HttpBase
{
    #region Constructors

    public ReadToDoListWf(HttpClient http) : base(http) { }

    #endregion

    #region Nested Classes

    public record InitAction(int Id, int Page);

    public record SuccessAction(PaginatedResponseDto<ToDoListItemSI> items);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListState OnInit(ToDoListState state, InitAction action)
    {
        return new ToDoListState(id: action.Id,
                                 isLoading: true,
                                 isCreating: state.IsCreating,
                                 items: state.Items);
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(InitAction action, IDispatcher dispatcher)
    {
        var pageData = await this.Http.ReadToDoListItemsAsync<ToDoListItemSI>(action.Id, action.Page);

        dispatcher.Dispatch(new SuccessAction(pageData));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ToDoListState OnSuccess(ToDoListState state, SuccessAction action)
    {
        return new ToDoListState(id: state.Id,
                                 isLoading: false,
                                 isCreating: state.IsCreating,
                                 items: action.items);
    }
}