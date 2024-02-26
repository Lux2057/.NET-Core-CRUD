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
public class TagsController : DispatcherControllerBase
{
    #region Constructors

    public TagsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     Route("~/" + ApiRoutes.ReadTags),
     ProducesResponseType(typeof(TagDto[]), 200)]
    public async Task<IActionResult> Get([FromQuery(Name = ApiRoutes.Params.SearchTerm)] string searchTerm, CancellationToken cancellationToken = default)
    {
        return Ok(await Dispatcher.QueryAsync(new GetTagsQuery(searchTerm), cancellationToken));
    }

    [HttpPost,
     Route("~/" + ApiRoutes.CreateTag),
     ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> Create([FromBody] CreateTagRequest request, CancellationToken cancellationToken = default)
    {
        var command = new CreateTagCommand(name: request.Name);
        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }
}