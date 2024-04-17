namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;

#endregion

public class FetchTasksWf
{
    #region Properties

    private readonly TasksAPI api;

    #endregion

    #region Constructors

    public FetchTasksWf(HttpClient http,
                        IDispatcher dispatcher)
    {
        this.api = new TasksAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init(int ProjectId,
                       int Page,
                       Action Callback = default) : IAuthRequiredAction
    {
        #region Properties

        public string AccessToken { get; set; }

        #endregion
    }

    public record Update(TaskStateDto[] Tasks,
                         Action Callback);

    #endregion

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnInit(TasksPageState state, Init action)
    {
        return new TasksPageState(isLoading: true,
                                  isCreating: state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: state.Tasks);
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var apiResponse = await this.api.GetAsync(projectId: action.ProjectId,
                                                  page: action.Page,
                                                  accessToken: action.AccessToken);

        dispatcher.Dispatch(new Update(apiResponse, action.Callback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static TasksPageState OnUpdate(TasksPageState state, Update action)
    {
        return new TasksPageState(isLoading: false,
                                  isCreating: state.IsCreating,
                                  projectId: state.ProjectId,
                                  tasks: action.Tasks);
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher _)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}