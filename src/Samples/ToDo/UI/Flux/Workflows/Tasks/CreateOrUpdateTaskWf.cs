namespace Samples.ToDo.UI;

#region << Using >>

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

#endregion

public class CreateOrUpdateTaskWf
{
    #region Properties

    private readonly TasksAPI tasksAPI;

    #endregion

    #region Constructors

    public CreateOrUpdateTaskWf(HttpClient http,
                                IDispatcher dispatcher)
    {
        this.tasksAPI = new TasksAPI(http, dispatcher);
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

    public record UpdateFail(int? TaskId);

    public record UpdateCreatingSuccess(Action Callback);

    public record UpdateEditingSuccess(TaskStateDto Task, Action Callback);

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
                                                   new PaginatedResponseDto<TaskStateDto>
                                                   {
                                                       Items = state.Tasks.Items.Select(r =>
                                                                                        {
                                                                                            if (r.Id == action.Request.Id)
                                                                                                r.IsUpdating = true;

                                                                                            return r;
                                                                                        }).ToArray(),
                                                       PagingInfo = state.Tasks.PagingInfo
                                                   });
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var success = await this.tasksAPI.CreateOrUpdateAsync(request: action.Request,
                                                              accessToken: action.AccessToken,
                                                              validationKey: action.ValidationKey);

        if (!success)
        {
            dispatcher.Dispatch(new UpdateFail(action.Request.Id));
            return;
        }

        if (action.Request.Id == null)
        {
            dispatcher.Dispatch(new UpdateCreatingSuccess(action.SuccessCallback));
            return;
        }

        dispatcher.Dispatch(new UpdateEditingSuccess(Task: new TaskStateDto
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
    public static TasksPageState OnUpdateFail(TasksPageState state, UpdateFail action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                    isCreating: false,
                                    projectId: state.ProjectId,
                                    tasks: action.TaskId == null ?
                                                   state.Tasks :
                                                   new PaginatedResponseDto<TaskStateDto>
                                                   {
                                                       Items = state.Tasks.Items.Select(r =>
                                                                                        {
                                                                                            if (r.Id == action.TaskId)
                                                                                                r.IsUpdating = false;

                                                                                            return r;
                                                                                        }).ToArray(),
                                                       PagingInfo = state.Tasks.PagingInfo
                                                   });
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnUpdateCreatingSuccess(TasksPageState state, UpdateCreatingSuccess action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                    isCreating: false,
                                    projectId: state.ProjectId,
                                    tasks: state.Tasks);
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdateCreatingSuccess(UpdateCreatingSuccess action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnUpdateEditingSuccess(TasksPageState state, UpdateEditingSuccess action)
    {
        return new TasksPageState(isLoading: state.IsLoading,
                                    isCreating: state.IsCreating,
                                    projectId: state.ProjectId,
                                    tasks: new PaginatedResponseDto<TaskStateDto>
                                    {
                                        Items = state.Tasks.Items.Select(r => r.Id == action.Task.Id ? action.Task : r).ToArray(),
                                        PagingInfo = state.Tasks.PagingInfo
                                    });
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdateEditingSuccess(UpdateEditingSuccess action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}