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

    public FetchProjectsWf(HttpClient http)
    {
        this.api = new ProjectsAPI(http);
    }

    #endregion

    #region Nested Classes

    public record Init(int Page, string SearchTerm = default, Action Callback = default) : IAuthenticatedAction, IValidationAPI
    {
        #region Properties

        public string AccessToken { get; set; }

        public string ValidationKey { get; set; }

        #endregion
    }

    public record Update(PaginatedResponseDto<ProjectEditableDto> Projects, Action Callback);

    #endregion

    [ReducerMethod]
    [UsedImplicitly]
    public static ProjectsState OnInit(ProjectsState state, Init action)
    {
        return new ProjectsState(isLoading: true,
                                 isCreating: state.IsCreating,
                                 projects: state.Projects);
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SetValidationStateWf.Init(action.ValidationKey, null));

        var apiResponse = await this.api.GetAsync(searchTerm: action.SearchTerm,
                                                  page: action.Page,
                                                  accessToken: action.AccessToken,
                                                  validationFailCallback: result => dispatcher.Dispatch(new SetValidationStateWf.Init(action.ValidationKey, result)));

        dispatcher.Dispatch(new Update(apiResponse, action.Callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ProjectsState OnUpdate(ProjectsState state, Update action)
    {
        return new ProjectsState(isLoading: false,
                                 isCreating: state.IsCreating,
                                 projects: action.Projects);
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher _)
    {
        action.Callback?.Invoke();
        return Task.CompletedTask;
    }
}