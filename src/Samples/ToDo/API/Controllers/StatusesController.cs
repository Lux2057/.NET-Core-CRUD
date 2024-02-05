namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

[Authorize,
 Route("[controller]/[action]")]
public class StatusesController : DispatcherControllerBase
{
    #region Constructors

    public StatusesController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     ProducesResponseType(typeof(StatusDto[]), 200)]
    public async Task<IActionResult> Get()
    {
        var currentUser = await Dispatcher.QueryAsync(new GetCurrentUserOrDefaultQuery());

        return Ok(await Dispatcher.QueryAsync(new GetStatusesQuery(currentUser?.Id ?? 0)));
    }

    [HttpPost,
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Create([FromBody] NamedRequest dto)
    {
        var currentUser = await Dispatcher.QueryAsync(new GetCurrentUserOrDefaultQuery());

        var command = new CreateOrUpdateStatusCommand(null, currentUser?.Id ?? 0, dto.Name);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPut,
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Update([FromBody] StatusDto dto)
    {
        var currentUser = await Dispatcher.QueryAsync(new GetCurrentUserOrDefaultQuery());

        var command = new CreateOrUpdateStatusCommand(dto.Id, currentUser.Id, dto.Name);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}