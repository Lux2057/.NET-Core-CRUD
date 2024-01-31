namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;

#endregion

[Route("[controller]/[action]")]
public class UsersController : DispatcherControllerBase
{
    #region Constructors

    public UsersController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] UserAuthDto dto)
    {
        var command = new CreateUserCommand(dto);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpDelete]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Delete([FromBody] int[] ids)
    {
        await Dispatcher.PushAsync(new DeleteEntitiesCommand<UserEntity>(ids));

        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var login = await Dispatcher.QueryAsync(new GetLoginQuery(id));

        return Ok(login);
    }
}