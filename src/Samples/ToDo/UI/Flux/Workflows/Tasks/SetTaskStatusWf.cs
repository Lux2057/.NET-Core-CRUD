namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class SetTaskStatusWf
{
    #region Properties

    private readonly TasksAPI api;

    #endregion

    #region Constructors

    public SetTaskStatusWf(HttpClient http,
                           IDispatcher dispatcher)
    {
        this.api = new TasksAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init : ValidatingAction<SetTaskStatusRequest>, IAuthRequiredAction
    {
        #region Properties

        public Action SuccessCallback { get; }

        public Action FailCallback { get; }

        public string AccessToken { get; set; }

        #endregion

        #region Constructors

        public Init(SetTaskStatusRequest request,
                    Action successCallback = default,
                    Action failCallback = default,
                    string validationKey = default)
                : base(request, validationKey)
        {
            SuccessCallback = successCallback;
            FailCallback = failCallback;
        }

        #endregion
    }

    public record Success(SetTaskStatusRequest Request,
                          Action Callback);

    public record Fail(SetTaskStatusRequest Request,
                       Action Callback);

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
                                                                    r.IsUpdating = true;

                                                                return r;
                                                            }).ToArray());
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var success = await this.api.SetStatusAsync(request: action.Request,
                                                    accessToken: action.AccessToken,
                                                    validationKey: action.ValidationKey);

        if (success)
            dispatcher.Dispatch(new Success(action.Request, action.SuccessCallback));
        else
            dispatcher.Dispatch(new Fail(action.Request, action.FailCallback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnSuccess(TasksPageState state, Success action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: state.Tasks.Select(r =>
                                                            {
                                                                if (r.Id == action.Request.Id)
                                                                {
                                                                    r.IsUpdating = false;
                                                                    r.Status = action.Request.Status;
                                                                }

                                                                return r;
                                                            }).ToArray());
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleSuccess(Success action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnFail(TasksPageState state, Fail action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: state.Tasks.Select(r =>
                                                            {
                                                                if (r.Id == action.Request.Id)
                                                                    r.IsUpdating = false;

                                                                return r;
                                                            }).ToArray());
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleFail(Fail action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}