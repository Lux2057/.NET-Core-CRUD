namespace Samples.ToDo.UI;

#region << Using >>

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

#endregion

public class DeleteTaskWf
{
    #region Properties

    private readonly TasksAPI api;

    #endregion

    #region Constructors

    public DeleteTaskWf(HttpClient http,
                        IDispatcher dispatcher)
    {
        this.api = new TasksAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init : ValidatingAction<DeleteEntityRequest>, IAuthRequiredAction
    {
        #region Properties

        public Action<bool> Callback { get; }

        public string AccessToken { get; set; }

        #endregion

        #region Constructors

        public Init(DeleteEntityRequest request,
                    Action<bool> callback,
                    string validationKey = default)
                : base(request, validationKey)
        {
            Callback = callback;
        }

        #endregion
    }

    public record Update(DeleteEntityRequest Request,
                         bool Success,
                         Action<bool> Callback);

    #endregion

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnInit(TasksPageState state, Init action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: state.Tasks.Select(r =>
                                                            {
                                                                if (r.Id == action.Request.Id)
                                                                    r.IsDeleting = true;

                                                                return r;
                                                            }).ToArray());
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var success = await this.api.DeleteAsync(request: action.Request,
                                                 accessToken: action.AccessToken,
                                                 validationKey: action.ValidationKey);

        dispatcher.Dispatch(new Update(action.Request, success, action.Callback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnUpdate(TasksPageState state, Update action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: action.Success ?
                                                 state.Tasks.Where(r => r.Id != action.Request.Id).ToArray() :
                                                 state.Tasks.Select(r =>
                                                                    {
                                                                        if (r.Id == action.Request.Id)
                                                                            r.IsDeleting = false;

                                                                        return r;
                                                                    }).ToArray());
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke(action.Success);

        return Task.CompletedTask;
    }
}