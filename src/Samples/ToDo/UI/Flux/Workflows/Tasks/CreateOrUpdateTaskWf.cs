namespace Samples.ToDo.UI;

using CRUD.Core;

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

        public Action<bool> Callback { get; }

        public string AccessToken { get; set; }

        #endregion

        #region Constructors

        public Init(CreateOrUpdateTaskRequest request,
                    Action<bool> callback = default,
                    string validationKey = default)
                : base(request, validationKey)
        {
            Callback = callback;
        }

        #endregion
    }

    public record Update(Init InitAction, bool Success);

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

        dispatcher.Dispatch(new Update(action, success));
    }

        [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnUpdate(TasksPageState state, Update action)
    {
        var isCreating = action.InitAction.Request.Id == null;

        return new TasksPageState(isLoading: state.IsLoading,
                                  isCreating: !isCreating && state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: isCreating ?
                                                    state.Tasks :
                                                    state.Tasks.Select(r =>
                                                                       {
                                                                           if (r.Id != action.InitAction.Request.Id)
                                                                               return r;

                                                                           r.IsUpdating = false;

                                                                           if (action.Success)
                                                                           {
                                                                               r.Name = action.InitAction.Request.Name;
                                                                               r.Description = action.InitAction.Request.Description;
                                                                           }

                                                                           return r;
                                                                       }).ToArray());
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher dispatcher)
    {
        action.InitAction.Callback?.Invoke(action.Success);

        return Task.CompletedTask;
    }
}