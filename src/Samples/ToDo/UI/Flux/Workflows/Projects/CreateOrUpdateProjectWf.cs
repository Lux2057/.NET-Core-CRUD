namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class CreateOrUpdateProjectWf
{
    #region Properties

    private readonly ProjectsAPI projectsAPI;

    #endregion

    #region Constructors

    public CreateOrUpdateProjectWf(HttpClient http,
                                   IDispatcher dispatcher)
    {
        this.projectsAPI = new ProjectsAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init : ValidatingAction<CreateOrUpdateProjectRequest>, IAuthRequiredAction
    {
        #region Properties

        public Action<bool> Callback { get; }

        public string AccessToken { get; set; }

        #endregion

        #region Constructors

        public Init(CreateOrUpdateProjectRequest request,
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
    public static ProjectsPageState OnInit(ProjectsPageState state, Init action)
    {
        var isCreating = action.Request.Id == null;

        return new ProjectsPageState(isLoading: state.IsLoading,
                                     isCreating: isCreating,
                                     projects: isCreating ?
                                                       state.Projects :
                                                       new PaginatedResponseDto<ProjectStatedDto>
                                                       {
                                                               Items = state.Projects.Items.Select(r =>
                                                                                                   {
                                                                                                       if (r.Id == action.Request.Id)
                                                                                                           r.IsUpdating = true;

                                                                                                       return r;
                                                                                                   }).ToArray(),
                                                               PagingInfo = state.Projects.PagingInfo
                                                       });
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var success = await this.projectsAPI.CreateOrUpdateAsync(request: action.Request,
                                                                 accessToken: action.AccessToken,
                                                                 validationKey: action.ValidationKey);

        dispatcher.Dispatch(new Update(action, success));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static ProjectsPageState OnUpdate(ProjectsPageState state, Update action)
    {
        var isCreating = action.InitAction.Request.Id == null;

        return new ProjectsPageState(isLoading: state.IsLoading,
                                     isCreating: !isCreating && state.IsCreating,
                                     projects: isCreating ?
                                                       state.Projects :
                                                       new PaginatedResponseDto<ProjectStatedDto>
                                                       {
                                                               Items = state.Projects.Items.Select(r =>
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
                                                                                                   }).ToArray(),
                                                               PagingInfo = state.Projects.PagingInfo
                                                       });
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher dispatcher)
    {
        action.InitAction.Callback?.Invoke(action.Success);

        return Task.CompletedTask;
    }
}