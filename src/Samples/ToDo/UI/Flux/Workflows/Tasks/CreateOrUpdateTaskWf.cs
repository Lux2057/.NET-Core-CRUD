namespace Samples.ToDo.UI;

#region << Using >>

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

#endregion

public class CreateOrUpdateTaskWf
{
    #region Properties

    private readonly TasksAPI api;

    #endregion

    #region Constructors

    public CreateOrUpdateTaskWf(HttpClient http,
                                IDispatcher dispatcher)
    {
        this.api = new TasksAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init : ValidatingAction<CreateOrUpdateTaskRequest>, IAuthRequiredAction
    {
        #region Properties

        public Action SuccessCallback { get; }

        public string AccessToken { get; set; }

        #endregion

        #region Constructors

        public Init(CreateOrUpdateTaskRequest request,
                    Action successCallback = default,
                    string validationKey = default)
                : base(request, validationKey)
        {
            SuccessCallback = successCallback;
        }

        #endregion
    }

    public record Fail(int? TaskId);

    public record CreatingSuccess(Action Callback);

    public record EditingSuccess(TaskStateDto Task, Action Callback);

    #endregion

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnInit(TasksPageState state, Init action)
    {
        if (action.Request.ProjectId != state.ProjectId)
            throw new Exception($"Invalid ProjectId: {state.ProjectId} is expected but {action.Request.ProjectId} was sent");

        var isCreating = action.Request.Id == null;

        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: isCreating,
                                  projectId: state.ProjectId,
                                  tasks: isCreating ?
                                                 state.Tasks :
                                                 state.Tasks.Select(r =>
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
        var success = await this.api.CreateOrUpdateAsync(request: action.Request,
                                                         accessToken: action.AccessToken,
                                                         validationKey: action.ValidationKey);

        if (!success)
        {
            dispatcher.Dispatch(new Fail(action.Request.Id));
            return;
        }

        if (action.Request.Id == null)
        {
            dispatcher.Dispatch(new CreatingSuccess(action.SuccessCallback));
            return;
        }

        dispatcher.Dispatch(new EditingSuccess(Task: new TaskStateDto
                                                     {
                                                             Id = action.Request.Id.GetValueOrDefault(),
                                                             Name = action.Request.Name,
                                                             Description = action.Request.Description,
                                                             IsUpdating = false
                                                     },
                                               Callback: action.SuccessCallback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnFail(TasksPageState state, Fail action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: false,
                                  projectId: state.ProjectId,
                                  tasks: action.TaskId == null ?
                                                 state.Tasks :
                                                 state.Tasks.Select(r =>
                                                                    {
                                                                        if (r.Id == action.TaskId)
                                                                            r.IsUpdating = false;

                                                                        return r;
                                                                    }).ToArray());
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnCreatingSuccess(TasksPageState state, CreatingSuccess action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: false,
                                  projectId: state.ProjectId,
                                  tasks: state.Tasks);
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleCreatingSuccess(CreatingSuccess action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnEditingSuccess(TasksPageState state, EditingSuccess action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: state.Tasks.Select(r => r.Id == action.Task.Id ? action.Task : r).ToArray());
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleEditingSuccess(EditingSuccess action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}