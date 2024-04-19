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
                       IDispatcher dispatcher)
            : base(http, dispatcher) { }

    #endregion

    public async Task<PaginatedResponseDto<ProjectStateDto>> ReadAsync(int page,
                                                                       string accessToken,
                                                                       CancellationToken cancellationToken = default)
    {
        var uri = $"{ApiRoutes.ReadProjects}?{ApiRoutes.Params.page}={page}";

        var result = await this.Http.GetApiResponseOrDefaultAsync
                             <PaginatedResponseDto<ProjectStateDto>>(dispatcher: this.dispatcher,
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
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.POST,
                                                                  uri: ApiRoutes.CreateOrUpdateProject,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }

    public async Task<bool> DeleteAsync(DeleteEntityRequest request,
                                        string accessToken,
                                        string validationKey,
                                        CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<bool>(dispatcher: this.dispatcher,
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.DELETE,
                                                                  uri: ApiRoutes.DeleteProject,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }
}