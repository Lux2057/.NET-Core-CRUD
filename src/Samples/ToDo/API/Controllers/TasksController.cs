namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samples.ToDo.Shared;

#endregion

[Authorize,
 Route("[controller]/[action]")]
public class TasksController : DispatcherControllerBase
{
    #region Constructors

    public TasksController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     Route("~/" + ApiRoutes.ReadTasks),
     ProducesResponseType(typeof(TaskDto[]), 200)]
    public async Task<IActionResult> Get([FromQuery(Name = ApiRoutes.Params.ProjectId)] int projectId,
                                         [FromQuery(Name = ApiRoutes.Params.SearchTerm)]
                                         string searchTerm,
                                         [FromQuery(Name = ApiRoutes.Params.TagsIds)]
                                         int[] tagsIds,
                                         CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        return Ok(await Dispatcher.QueryAsync(new GetTasksQuery(userId: currentUserId,
                                                                projectId: projectId,
                                                                searchTerm: searchTerm,
                                                                tagsIds: tagsIds), cancellationToken));
    }

    [HttpPost,
     Route("~/" + ApiRoutes.CreateOrUpdateTask),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        var command = new CreateOrUpdateTaskCommand(id: request.Id,
                                                    userId: currentUserId,
                                                    statusId: request.StatusId,
                                                    name: request.Name,
                                                    projectId: request.ProjectId,
                                                    description: request.Description,
                                                    dueDate: request.DueDate,
                                                    tagsIds: request.TagsIds);

        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }

    [HttpPut,
     Route("~/" + ApiRoutes.SetTaskStatus),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> SetStatus([FromBody] SetTaskStatusRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);
        await Dispatcher.PushAsync(new SetTaskStatusCommand(id: request.Id,
                                                            userId: currentUserId,
                                                            statusId: request.StatusId), cancellationToken);

        return Ok();
    }
}