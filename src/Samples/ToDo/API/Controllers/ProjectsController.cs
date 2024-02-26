namespace Samples.ToDo.API;

#region << Using >>

using CRUD.Core;
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
     Route("~/" + ApiRoutes.ReadProjects),
     ProducesResponseType(typeof(PaginatedResponseDto<ProjectDto>), 200)]
    public async Task<IActionResult> Read([FromQuery(Name = ApiRoutes.Params.SearchTerm)] string searchTerm,
                                          [FromQuery(Name = ApiRoutes.Params.TagsIds)]
                                          int[] tagsIds,
                                          [FromQuery(Name = ApiRoutes.Params.page)]
                                          int? page,
                                          [FromQuery(Name = ApiRoutes.Params.pageSize)]
                                          int? pageSize,
                                          CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        return Ok(await Dispatcher.QueryAsync(new GetProjectsQuery(userId: currentUserId,
                                                                   searchTerm: searchTerm,
                                                                   tagsIds: tagsIds,
                                                                   disablePaging: false,
                                                                   page: page,
                                                                   pageSize: pageSize), cancellationToken));
    }

    [HttpPost,
     Route("~/" + ApiRoutes.CreateOrUpdateProject),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        var command = new CreateOrUpdateProjectCommand(id: request.Id,
                                                       userId: currentUserId,
                                                       name: request.Name,
                                                       description: request.Description,
                                                       tagsIds: request.TagsIds);

        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }
}