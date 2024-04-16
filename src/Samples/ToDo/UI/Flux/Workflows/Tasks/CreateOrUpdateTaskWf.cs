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

    private readonly TagsAPI tagsAPI;

    private readonly TasksAPI tasksAPI;

    #endregion

    #region Constructors

    public CreateOrUpdateTaskWf(HttpClient http,
                                IDispatcher dispatcher)
    {
        this.tasksAPI = new TasksAPI(http, dispatcher);
        this.tagsAPI = new TagsAPI(http, dispatcher);
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
    public static TasksState OnInit(TasksState state, Init action)
    {
        if (action.Request.ProjectId != state.ProjectId)
            throw new Exception($"Invalid ProjectId: {state.ProjectId} is expected but {action.Request.ProjectId} was sent");

        var isCreating = action.Request.Id == null;

        return new TasksState(isLoading: state.IsLoading,
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

        var tags = action.Request.TagsIds.Any() ?
                           await this.tagsAPI.GetAsync(ids: action.Request.TagsIds,
                                                       validationKey: action.ValidationKey,
                                                       accessToken: action.AccessToken) :
                           Array.Empty<TagDto>();

        dispatcher.Dispatch(new UpdateEditingSuccess(Task: new TaskStateDto
                                                           {
                                                                   Id = action.Request.Id.GetValueOrDefault(),
                                                                   Name = action.Request.Name,
                                                                   Description = action.Request.Description,
                                                                   Tags = tags,
                                                                   IsUpdating = false
                                                           },
                                                     Callback: action.SuccessCallback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksState OnUpdateFail(TasksState state, UpdateFail action)
    {
        return new TasksState(isLoading: state.IsLoading,
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
    public static TasksState OnUpdateCreatingSuccess(TasksState state, UpdateCreatingSuccess action)
    {
        return new TasksState(isLoading: state.IsLoading,
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
    public static TasksState OnUpdateEditingSuccess(TasksState state, UpdateEditingSuccess action)
    {
        return new TasksState(isLoading: state.IsLoading,
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