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

    private readonly ProjectsAPI api;

    #endregion

    #region Constructors

    public CreateOrUpdateProjectWf(HttpClient http, 
                                   IDispatcher dispatcher,
                                   IState<LocalizationState> localizationState)
    {
        this.api = new ProjectsAPI(http, dispatcher, localizationState);
    }

    #endregion

    #region Nested Classes

    public record Init(ProjectDto Project,
                       bool IsUpdate,
                       Action Callback = default) : IAuthenticatedAction, IValidatingAction
    {
        #region Properties

        public string AccessToken { get; set; }

        public string ValidationKey { get; set; }

        #endregion
    }

    public record Update(ProjectDto Project, Action Callback);

    #endregion

    static PaginatedResponseDto<ProjectEditableDto> copy(PaginatedResponseDto<ProjectEditableDto> projects, ProjectDto dto, bool isUpdating)
    {
        return new PaginatedResponseDto<ProjectEditableDto>
               {
                       Items = projects.Items.Select(r =>
                                                     {
                                                         if (r.Id == dto.Id)
                                                         {
                                                             r.IsUpdating = isUpdating;
                                                             r.Name = dto.Name;
                                                         }

                                                         return r;
                                                     }).ToArray(),
                       PagingInfo = projects.PagingInfo
               };
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ProjectsState OnInit(ProjectsState state, Init action)
    {
        var isCreating = state.Projects.Items.All(r => r.Id != action.Project.Id);

        return new ProjectsState(isLoading: state.IsLoading,
                                 isCreating: isCreating,
                                 projects: copy(state.Projects, action.Project, true));
    }

    [EffectMethod]
    [UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        if (action.IsUpdate)
            await this.api.UpdateAsync(request: new EditProjectRequest
                                                {
                                                        Id = action.Project.Id,
                                                        Name = action.Project.Name,
                                                        Description = action.Project.Description,
                                                        TagsIds = action.Project.Tags.Select(r => r.Id).ToArray()
                                                },
                                       accessToken: action.AccessToken,
                                       validationKey: action.ValidationKey);
        else
            await this.api.CreateAsync(request: new CreateProjectRequest
                                                {
                                                        Name = action.Project.Name,
                                                        Description = action.Project.Description,
                                                        TagsIds = action.Project.Tags.Select(r => r.Id).ToArray()
                                                },
                                       accessToken: action.AccessToken,
                                       validationKey: action.ValidationKey);

        dispatcher.Dispatch(new Update(action.Project, action.Callback));
    }

    [ReducerMethod]
    [UsedImplicitly]
    public static ProjectsState OnSuccess(ProjectsState state, Update action)
    {
        return new ProjectsState(isLoading: state.IsLoading,
                                 isCreating: false,
                                 projects: copy(state.Projects, action.Project, false));
    }

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleSuccess(Update action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}