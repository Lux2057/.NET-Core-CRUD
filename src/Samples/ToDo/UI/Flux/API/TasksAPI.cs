namespace Samples.ToDo.UI;

#region << Using >>

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

    public async Task<TaskStateDto[]> GetAsync(int projectId,
                                               string accessToken,
                                               CancellationToken cancellationToken = default)
    {
        var uri = $"{ApiRoutes.ReadTasks}?{ApiRoutes.Params.ProjectId}={projectId}";

        var result = await this.Http.GetApiResponseOrDefaultAsync<TaskStateDto[]>(dispatcher: this.dispatcher,
                                                                                  validationKey: null,
                                                                                  httpMethod: HttpMethodType.GET,
                                                                                  uri: uri,
                                                                                  accessToken: accessToken,
                                                                                  cancellationToken: cancellationToken);

        return result ?? Array.Empty<TaskStateDto>();
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

    public async Task<bool> DeleteAsync(DeleteEntityRequest request,
                                        string accessToken,
                                        string validationKey,
                                        CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<bool>(dispatcher: this.dispatcher,
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.DELETE,
                                                                  uri: ApiRoutes.DeleteTask,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }

    public async Task<bool> SetStatusAsync(SetTaskStatusRequest request,
                                           string accessToken,
                                           string validationKey,
                                           CancellationToken cancellationToken = default)
    {
        return await this.Http.GetApiResponseOrDefaultAsync<bool>(dispatcher: this.dispatcher,
                                                                  validationKey: validationKey,
                                                                  httpMethod: HttpMethodType.PUT,
                                                                  uri: ApiRoutes.SetTaskStatus,
                                                                  accessToken: accessToken,
                                                                  content: request,
                                                                  cancellationToken: cancellationToken);
    }
}