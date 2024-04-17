namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;

#endregion

public class FetchProjectsWf
{
    #region Properties

    private readonly ProjectsAPI api;

    #endregion

    #region Constructors

    public FetchProjectsWf(HttpClient http,
                           IDispatcher dispatcher)
    {
        this.api = new ProjectsAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init(int Page,
                       string SearchTerm = default,
                       Action Callback = default) : IAuthRequiredAction
    {
        #region Properties

        public string AccessToken { get; set; }

        #endregion
    }

    public record Update(PaginatedResponseDto<ProjectStateDto> Projects,
                         Action Callback);

    #endregion

    [ReducerMethod,
     UsedImplicitly]
    public static ProjectsPageState OnInit(ProjectsPageState pageState, Init action)
    {
        return new ProjectsPageState(isLoading: true,
                                 isCreating: pageState.IsCreating,
                                 projects: pageState.Projects);
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var apiResponse = await this.api.GetAsync(searchTerm: action.SearchTerm,
                                                  page: action.Page,
                                                  accessToken: action.AccessToken);

        dispatcher.Dispatch(new Update(apiResponse, action.Callback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static ProjectsPageState OnUpdate(ProjectsPageState pageState, Update action)
    {
        return new ProjectsPageState(isLoading: false,
                                 isCreating: pageState.IsCreating,
                                 projects: action.Projects);
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher _)
    {
        action.Callback?.Invoke();
        return Task.CompletedTask;
    }
}