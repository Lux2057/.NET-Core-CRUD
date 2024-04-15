namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using Samples.ToDo.Shared;

#endregion

public class ProjectsAPI : ApiBase
{
    #region Constructors

    public ProjectsAPI(HttpClient http,
                       IDispatcher dispatcher,
                       IState<LanguageState> localizationState)
            : base(http, dispatcher, localizationState) { }

    #endregion

    public async Task<PaginatedResponseDto<ProjectStateDto>> GetAsync(string searchTerm,
                                                                      int page,
                                                                      string accessToken,
                                                                      int[] tagsIds = default,
                                                                      CancellationToken cancellationToken = default)
    {
        var uri = $"{ApiRoutes.ReadProjects}?"
                + $"{ApiRoutes.Params.SearchTerm}={searchTerm}&"
                + $"{ApiRoutes.Params.page}={page}&";

        if (tagsIds?.Any() == true)
            uri += $"&{tagsIds.ToApiParams(ApiRoutes.Params.TagsIds)}";

        var result = await this.Http.GetApiResponseOrDefaultAsync<PaginatedResponseDto<ProjectStateDto>>(dispatcher: this.dispatcher,
                                                                                                         acceptLanguage: this.localizationState.Value.Language,
                                                                                                         validationKey: null,
                                                                                                         httpMethod: HttpMethodType.GET,
                                                                                                         uri: uri,
                                                                                                         accessToken: accessToken,
                                                                                                         cancellationToken: cancellationToken);

        return result ?? new PaginatedResponseDto<ProjectStateDto>();
    }

    public async Task<bool> CreateOrUpdateAsync(CreateOrUpdateProjectRequest request,
                                                string accessToken,
                                                string validationKey,
                                                CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<bool>(dispatcher: this.dispatcher,
                                                                  acceptLanguage: this.localizationState.Value.Language,
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.POST,
                                                                  uri: ApiRoutes.CreateOrUpdateProject,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }
}