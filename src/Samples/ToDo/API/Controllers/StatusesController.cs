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
public class StatusesController : DispatcherControllerBase
{
    #region Constructors

    public StatusesController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     Route("~/" + ApiRoutes.ReadStatuses),
     ProducesResponseType(typeof(StatusDto[]), 200)]
    public async Task<IActionResult> Read([FromQuery(Name = ApiRoutes.Params.SearchTerm)] string searchTerm, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        return Ok(await Dispatcher.QueryAsync(new GetStatusesQuery(userId: currentUserId,
                                                                   searchTerm: searchTerm), cancellationToken));
    }

    [HttpPost,
     Route("~/" + ApiRoutes.CreateOrUpdateStatus),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateStatusRequest request, CancellationToken cancellationToken = default)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery(), cancellationToken);

        var command = new CreateOrUpdateStatusCommand(id: request.Id,
                                                      userId: currentUserId,
                                                      name: request.Name);

        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }
}