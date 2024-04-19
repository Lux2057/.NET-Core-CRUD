namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class DeleteProjectWf
{
    #region Properties

    private readonly ProjectsAPI api;

    #endregion

    #region Constructors

    public DeleteProjectWf(HttpClient http,
                           IDispatcher dispatcher)
    {
        this.api = new ProjectsAPI(http, dispatcher);
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
    public static ProjectsPageState OnInit(ProjectsPageState state, Init action)
    {
        return new ProjectsPageState(isLoading: state.IsLoading,
                                     isCreating: state.IsCreating,
                                     projects: new PaginatedResponseDto<ProjectStateDto>
                                               {
                                                       Items = state.Projects.Items.Select(r =>
                                                                                           {
                                                                                               if (r.Id == action.Request.Id)
                                                                                                   r.IsDeleting = true;

                                                                                               return r;
                                                                                           }).ToArray(),
                                                       PagingInfo = state.Projects.PagingInfo
                                               });
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
    public static ProjectsPageState OnUpdate(ProjectsPageState state, Update action)
    {
        return new ProjectsPageState(isLoading: state.IsLoading,
                                     isCreating: state.IsCreating,
                                     projects: new PaginatedResponseDto<ProjectStateDto>
                                               {
                                                       Items = action.Success ?
                                                                       state.Projects.Items.Where(r => r.Id != action.Request.Id).ToArray() :
                                                                       state.Projects.Items.Select(r =>
                                                                                                   {
                                                                                                       if (r.Id == action.Request.Id)
                                                                                                           r.IsDeleting = false;

                                                                                                       return r;
                                                                                                   }).ToArray(),
                                                       PagingInfo = state.Projects.PagingInfo
                                               });
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke(action.Success);

        return Task.CompletedTask;
    }
}