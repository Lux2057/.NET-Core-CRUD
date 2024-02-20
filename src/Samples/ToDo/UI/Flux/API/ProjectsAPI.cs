namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Samples.ToDo.Shared;

#endregion

public class ProjectsAPI : HttpBase
{
    #region Constructors

    public ProjectsAPI(HttpClient http) : base(http) { }

    #endregion

    public async Task<PaginatedResponseDto<ProjectEditableDto>> GetAsync(string searchTerm,
                                                                         int page,
                                                                         string accessToken,
                                                                         Action<ValidationFailureResult> ValidationFailCallback = null,
                                                                         int[] tagsIds = default)
    {
        var uri = $"{ApiRoutes.GetProjects}?"
                + $"{ApiRoutes.Params.SearchTerm}={searchTerm}&"
                + $"{ApiRoutes.Params.page}={page}&";

        if (tagsIds?.Any() == true)
            uri += $"&{tagsIds.ToApiParams(ApiRoutes.Params.TagsIds)}";

        var httpResponse = await this.Http.SendApiRequestAsync(httpMethod: HttpMethodType.GET,
                                                               uri: uri,
                                                               accessToken: accessToken);

        var result = await httpResponse.ToApiResponseOrDefaultAsync<PaginatedResponseDto<ProjectEditableDto>>(ValidationFailCallback);
        return result ?? new PaginatedResponseDto<ProjectEditableDto>();
    }

    public async Task<int> CreateAsync(CreateProjectRequest request,
                                       string accessToken,
                                       Action<ValidationFailureResult> ValidationFailCallback = null)
    {
        var httpResponse = await this.Http.SendApiRequestAsync(httpMethod: HttpMethodType.POST,
                                                               uri: ApiRoutes.CreateProject,
                                                               accessToken: accessToken,
                                                               content: request);

        return await httpResponse.ToApiResponseOrDefaultAsync<int>(ValidationFailCallback);
    }

    public async Task<int> UpdateAsync(EditProjectRequest request,
                                       string accessToken,
                                       Action<ValidationFailureResult> ValidationFailCallback = null)
    {
        var httpResponse = await this.Http.SendApiRequestAsync(httpMethod: HttpMethodType.PUT,
                                                               uri: ApiRoutes.UpdateProject,
                                                               accessToken: accessToken,
                                                               content: request);

        return await httpResponse.ToApiResponseOrDefaultAsync<int>(ValidationFailCallback);
    }
}