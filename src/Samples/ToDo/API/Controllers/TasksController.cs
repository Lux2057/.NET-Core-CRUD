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
     Route("~/" + ApiRoutesConst.GetTasks),
     ProducesResponseType(typeof(TaskDto[]), 200)]
    public async Task<IActionResult> Get([FromQuery(Name = ApiRoutesConst.Params.ProjectId)] int projectId,
                                         [FromQuery(Name = ApiRoutesConst.Params.SearchTerm)]
                                         string searchTerm,
                                         [FromQuery(Name = ApiRoutesConst.Params.TagsIds)]
                                         int[] tagsIds)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        return Ok(await Dispatcher.QueryAsync(new GetTasksQuery(userId: currentUserId,
                                                                projectId: projectId,
                                                                searchTerm: searchTerm,
                                                                tagsIds: tagsIds)));
    }

    [HttpPost,
     Route("~/" + ApiRoutesConst.CreateTask),
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
     Route("~/" + ApiRoutesConst.UpdateTask),
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

    [HttpPut,
     Route("~/" + ApiRoutesConst.SetTaskStatus),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> SetStatus([FromBody] TaskDto.SetStatusRequest request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new SetTaskStatusCommand(id: request.Id,
                                               userId: currentUserId,
                                               statusId: request.StatusId);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}