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
     Route($"~/{ApiRoutes.ReadTasks}"),
     ProducesResponseType(typeof(TaskDto[]), 200)]
    public async Task<IActionResult> Get([FromQuery(Name = ApiRoutes.Params.ProjectId)] int projectId,
                                         CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        return Ok(await Dispatcher.QueryAsync(new GetTasksQuery(userId: currentUserId,
                                                                projectId: projectId), cancellationToken));
    }

    [HttpPost,
     Route($"~/{ApiRoutes.CreateOrUpdateTask}"),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateTaskRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        var command = new CreateOrUpdateTaskCommand(id: request.Id,
                                                    userId: currentUserId,
                                                    status: request.Status,
                                                    name: request.Name,
                                                    projectId: request.ProjectId,
                                                    description: request.Description);

        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }

    [HttpPut,
     Route($"~/{ApiRoutes.SetTaskStatus}"),
     ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> SetStatus([FromBody] SetTaskStatusRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        var command = new SetTaskStatusCommand(id: request.Id,
                                               userId: currentUserId,
                                               status: request.Status);

        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }

    [HttpDelete,
     Route($"~/{ApiRoutes.DeleteTask}"),
     ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> Delete([FromBody] DeleteEntityRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        var command = new DeleteTaskCommand(id: request.Id,
                                            userId: currentUserId);

        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }
}