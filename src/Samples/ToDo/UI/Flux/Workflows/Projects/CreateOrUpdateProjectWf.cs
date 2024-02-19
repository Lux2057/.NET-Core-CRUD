namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;

#endregion

public class CreateOrUpdateProjectWf : HttpBase
{
    #region Properties

    private readonly ProjectsAPI api;

    #endregion

    #region Constructors

    public CreateOrUpdateProjectWf(HttpClient http,
                                   IStringLocalizer<Resource> localization,
                                   IDispatcher dispatcher)
            : base(http)
    {
        this.api = new ProjectsAPI(http, localization, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init(ProjectDto Project,
                       string AccessToken,
                       bool IsUpdate,
                       Action Callback = default);

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
            await this.api.UpdateAsync(new EditProjectRequest
                                       {
                                               Id = action.Project.Id,
                                               Name = action.Project.Name,
                                               Description = action.Project.Description,
                                               TagsIds = action.Project.Tags.Select(r => r.Id).ToArray()
                                       }, action.AccessToken);
        else
            await this.api.CreateAsync(new CreateProjectRequest
                                       {
                                               Name = action.Project.Name,
                                               Description = action.Project.Description,
                                               TagsIds = action.Project.Tags.Select(r => r.Id).ToArray()
                                       }, action.AccessToken);

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