namespace Samples.ToDo.UI;

#region << Using >>

using CRUD.Core;
using Fluxor;
using Samples.ToDo.Shared;

#endregion

public class TasksAPI : ApiBase
{
    #region Constructors

    public TasksAPI(HttpClient http,
                    IDispatcher dispatcher)
            : base(http, dispatcher) { }

    #endregion

    public async Task<PaginatedResponseDto<TaskStateDto>> GetAsync(int projectId,
                                                                   string searchTerm,
                                                                   int page,
                                                                   string accessToken,
                                                                   int[] tagsIds = default,
                                                                   CancellationToken cancellationToken = default)
    {
        var uri = $"{ApiRoutes.ReadTasks}?"
                + $"{ApiRoutes.Params.ProjectId}={projectId}&"
                + $"{ApiRoutes.Params.SearchTerm}={searchTerm}&"
                + $"{ApiRoutes.Params.page}={page}";

        if (tagsIds?.Any() == true)
            uri += $"&{tagsIds.ToApiParams(ApiRoutes.Params.TagsIds)}";

        var result = await this.Http.GetApiResponseOrDefaultAsync
                             <PaginatedResponseDto<TaskStateDto>>(dispatcher: this.dispatcher,
                                                                  validationKey: null,
                                                                  httpMethod: HttpMethodType.GET,
                                                                  uri: uri,
                                                                  accessToken: accessToken,
                                                                  cancellationToken: cancellationToken);

        return result ?? new PaginatedResponseDto<TaskStateDto>();
    }

    public async Task<bool> CreateOrUpdateAsync(CreateOrUpdateTaskRequest request,
                                                string accessToken,
                                                string validationKey,
                                                CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<bool>(dispatcher: this.dispatcher,
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.POST,
                                                                  uri: ApiRoutes.CreateOrUpdateTask,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }
}