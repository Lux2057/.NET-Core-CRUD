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

        public Action SuccessCallback { get; }

        public string AccessToken { get; set; }

        #endregion

        #region Constructors

        public Init(CreateOrUpdateProjectRequest request,
                    Action successCallback = default,
                    string validationKey = default)
                : base(request, validationKey)
        {
            SuccessCallback = successCallback;
        }

        #endregion
    }

    public record UpdateFail(int? ProjectId);

    public record UpdateCreatingSuccess(Action Callback);

    public record UpdateEditingSuccess(ProjectStateDto Project, Action Callback);

    #endregion

    [ReducerMethod,
     UsedImplicitly]
    public static ProjectsPageState OnInit(ProjectsPageState pageState, Init action)
    {
        var isCreating = action.Request.Id == null;

        return new ProjectsPageState(isLoading: pageState.IsLoading,
                                 isCreating: isCreating,
                                 projects: isCreating ?
                                                   pageState.Projects :
                                                   new PaginatedResponseDto<ProjectStateDto>
                                                   {
                                                           Items = pageState.Projects.Items.Select(r =>
                                                                                               {
                                                                                                   if (r.Id == action.Request.Id)
                                                                                                       r.IsUpdating = true;

                                                                                                   return r;
                                                                                               }).ToArray(),
                                                           PagingInfo = pageState.Projects.PagingInfo
                                                   });
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var success = await this.projectsAPI.CreateOrUpdateAsync(request: action.Request,
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

        dispatcher.Dispatch(new UpdateEditingSuccess(Project: new ProjectStateDto
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
    public static ProjectsPageState OnUpdateFail(ProjectsPageState pageState, UpdateFail action)
    {
        return new ProjectsPageState(isLoading: pageState.IsLoading,
                                 isCreating: false,
                                 projects: action.ProjectId == null ?
                                                   pageState.Projects :
                                                   new PaginatedResponseDto<ProjectStateDto>
                                                   {
                                                           Items = pageState.Projects.Items.Select(r =>
                                                                                               {
                                                                                                   if (r.Id == action.ProjectId)
                                                                                                       r.IsUpdating = false;

                                                                                                   return r;
                                                                                               }).ToArray(),
                                                           PagingInfo = pageState.Projects.PagingInfo
                                                   });
    }

    [ReducerMethod,
     UsedImplicitly]
    public static ProjectsPageState OnUpdateCreatingSuccess(ProjectsPageState pageState, UpdateCreatingSuccess action)
    {
        return new ProjectsPageState(isLoading: pageState.IsLoading,
                                 isCreating: false,
                                 projects: pageState.Projects);
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
    public static ProjectsPageState OnUpdateEditingSuccess(ProjectsPageState pageState, UpdateEditingSuccess action)
    {
        return new ProjectsPageState(isLoading: pageState.IsLoading,
                                 isCreating: pageState.IsCreating,
                                 projects: new PaginatedResponseDto<ProjectStateDto>
                                           {
                                                   Items = pageState.Projects.Items.Select(r => r.Id == action.Project.Id ? action.Project : r).ToArray(),
                                                   PagingInfo = pageState.Projects.PagingInfo
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