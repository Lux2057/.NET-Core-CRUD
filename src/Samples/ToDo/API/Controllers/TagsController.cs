namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

[Authorize,
 Route("[controller]/[action]")]
public class TagsController : DispatcherControllerBase
{
    #region Constructors

    public TagsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     ProducesResponseType(typeof(StatusDto[]), 200)]
    public async Task<IActionResult> Get([FromQuery] string searchTerm)
    {
        return Ok(await Dispatcher.QueryAsync(new GetTagsQuery(searchTerm)));
    }

    [HttpPost,
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Create([FromBody] TagDto.CreateRequest request)
    {
        var command = new CreateTagCommand(name: request.Name);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}