namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using Microsoft.Extensions.Localization;
using Samples.ToDo.Shared;
using Samples.ToDo.UI.Localization;

#endregion

public class ProjectsAPI : HttpBase
{
    #region Properties

    readonly IDispatcher dispatcher;

    readonly IStringLocalizer<Resource> localization;

    #endregion

    #region Constructors

    public ProjectsAPI(HttpClient http,
                       IStringLocalizer<Resource> localization,
                       IDispatcher dispatcher)
            : base(http)
    {
        this.localization = localization;
        this.dispatcher = dispatcher;
    }

    #endregion

    public async Task<PaginatedResponseDto<ProjectEditableDto>> GetAsync(string searchTerm,
                                                                         int page,
                                                                         string accessToken,
                                                                         int[] tagsIds = default)
    {
        var uri = $"{ApiRoutes.GetProjects}?"
                + $"{ApiRoutes.Params.SearchTerm}={searchTerm}&"
                + $"{ApiRoutes.Params.page}={page}&";

        if (tagsIds?.Any() == true)
            uri += $"&{tagsIds.ToApiParams(ApiRoutes.Params.TagsIds)}";

        var httpResponse = await this.Http.SendAuthenticatedRequestAsync(HttpMethodType.GET, uri, accessToken);

        return await httpResponse.ToApiResponseOrDefaultAsync<PaginatedResponseDto<ProjectEditableDto>>(this.dispatcher, this.localization);
    }

    public async Task<int> CreateAsync(CreateProjectRequest request, string accessToken)
    {
        var httpResponse = await this.Http.SendAuthenticatedRequestAsync(HttpMethodType.POST, ApiRoutes.CreateProject, accessToken, request);

        return await httpResponse.ToApiResponseOrDefaultAsync<int>(this.dispatcher, this.localization);
    }

    public async Task<int> UpdateAsync(EditProjectRequest request, string accessToken)
    {
        var httpResponse = await this.Http.SendAuthenticatedRequestAsync(HttpMethodType.PUT, ApiRoutes.UpdateProject, accessToken, request);

        return await httpResponse.ToApiResponseOrDefaultAsync<int>(this.dispatcher, this.localization);
    }
}