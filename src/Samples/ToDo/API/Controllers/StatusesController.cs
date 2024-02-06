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
     Route("~/" + ApiRoutesConst.GetStatuses),
     ProducesResponseType(typeof(StatusDto[]), 200)]
    public async Task<IActionResult> Get()
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        return Ok(await Dispatcher.QueryAsync(new GetStatusesQuery(currentUserId)));
    }

    [HttpPost,
     Route("~/" + ApiRoutesConst.CreateStatus),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Create([FromBody] StatusDto.CreateRequest request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new CreateOrUpdateStatusCommand(id: null,
                                                      userId: currentUserId,
                                                      name: request.Name);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPut,
     Route("~/" + ApiRoutesConst.UpdateStatus),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Update([FromBody] StatusDto.EditRequest request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new CreateOrUpdateStatusCommand(id: request.Id,
                                                      userId: currentUserId,
                                                      name: request.Name);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}