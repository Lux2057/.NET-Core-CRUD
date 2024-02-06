namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samples.ToDo.API.Projects;
using Samples.ToDo.Shared;

#endregion

[Authorize,
 Route("[controller]/[action]")]
public class ProjectsController : DispatcherControllerBase
{
    #region Constructors

    public ProjectsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     Route("~/" + ApiRoutesConst.GetProjects),
     ProducesResponseType(typeof(ProjectDto[]), 200)]
    public async Task<IActionResult> Get()
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        return Ok(await Dispatcher.QueryAsync(new GetProjectsQuery(currentUserId)));
    }

    [HttpPost,
     Route("~/" + ApiRoutesConst.CreateProject),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Create([FromBody] ProjectDto.CreateRequest request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new CreateOrUpdateProjectCommand(id: null,
                                                       userId: currentUserId,
                                                       name: request.Name,
                                                       description: request.Description,
                                                       tagsIds: request.TagsIds);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPut,
     Route("~/" + ApiRoutesConst.UpdateProject),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Update([FromBody] ProjectDto.EditRequest request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new CreateOrUpdateProjectCommand(id: request.Id,
                                                       userId: currentUserId,
                                                       name: request.Name,
                                                       description: request.Description,
                                                       tagsIds: request.TagsIds);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}