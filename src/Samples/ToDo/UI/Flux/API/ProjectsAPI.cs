namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using Samples.ToDo.Shared;

#endregion

public class ProjectsAPI : HttpBase
{
    #region Properties

    readonly IDispatcher dispatcher;

    #endregion

    #region Constructors

    public ProjectsAPI(HttpClient http, IDispatcher dispatcher) : base(http)
    {
        this.dispatcher = dispatcher;
    }

    #endregion

    public async Task<PaginatedResponseDto<ProjectEditableDto>> GetAsync(string searchTerm,
                                                                         int page,
                                                                         string accessToken,
                                                                         string validationKey,
                                                                         int[] tagsIds = default,
                                                                         CancellationToken cancellationToken = default)
    {
        var uri = $"{ApiRoutes.GetProjects}?"
                + $"{ApiRoutes.Params.SearchTerm}={searchTerm}&"
                + $"{ApiRoutes.Params.page}={page}&";

        if (tagsIds?.Any() == true)
            uri += $"&{tagsIds.ToApiParams(ApiRoutes.Params.TagsIds)}";

        var result = await this.Http.GetApiResponseOrDefaultAsync<PaginatedResponseDto<ProjectEditableDto>>(dispatcher: this.dispatcher,
                                                                                                            validationKey: validationKey,
                                                                                                            httpMethod: HttpMethodType.GET,
                                                                                                            uri: uri,
                                                                                                            accessToken: accessToken,
                                                                                                            cancellationToken: cancellationToken);

        return result ?? new PaginatedResponseDto<ProjectEditableDto>();
    }

    public async Task<int> CreateAsync(CreateProjectRequest request,
                                       string accessToken,
                                       string validationKey,
                                       CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<int>(dispatcher: this.dispatcher,
                                                                 validationKey: validationKey,
                                                                 httpMethod: HttpMethodType.POST,
                                                                 uri: ApiRoutes.CreateProject,
                                                                 accessToken: accessToken,
                                                                 content: request,
                                                                 cancellationToken: cancellationToken);
    }

    public async Task<int> UpdateAsync(EditProjectRequest request,
                                       string accessToken,
                                       string validationKey,
                                       CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<int>(dispatcher: this.dispatcher,
                                                                 validationKey: validationKey,
                                                                 httpMethod: HttpMethodType.PUT,
                                                                 uri: ApiRoutes.UpdateProject,
                                                                 accessToken: accessToken,
                                                                 content: request,
                                                                 cancellationToken: cancellationToken);
    }
}