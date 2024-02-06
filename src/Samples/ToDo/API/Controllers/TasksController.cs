namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

[Authorize,
 Route("[controller]/[action]")]
public class TasksController : DispatcherControllerBase
{
    #region Constructors

    public TasksController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     ProducesResponseType(typeof(TaskDto[]), 200)]
    public async Task<IActionResult> Get([FromQuery] int projectId)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        return Ok(await Dispatcher.QueryAsync(new GetTasksQuery(currentUserId, projectId)));
    }

    [HttpPost,
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Create([FromBody] TaskDto.CreateRequest request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new CreateOrUpdateTaskCommand(id: null,
                                                    userId: currentUserId,
                                                    name: request.Name,
                                                    projectId: request.ProjectId,
                                                    description: request.Description,
                                                    dueDate: request.DueDate,
                                                    tagsIds: request.TagsIds);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPut,
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Update([FromBody] TaskDto.EditRequest request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new CreateOrUpdateTaskCommand(id: request.Id,
                                                    userId: currentUserId,
                                                    name: request.Name,
                                                    projectId: request.ProjectId,
                                                    description: request.Description,
                                                    dueDate: request.DueDate,
                                                    tagsIds: request.TagsIds);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}