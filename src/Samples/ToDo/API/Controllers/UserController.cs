namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;

#endregion

[Route("[controller]/[action]")]
public class UserController : DispatcherControllerBase
{
    #region Constructors

    public UserController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] UserAuthDto dto)
    {
        var command = new CreateUserCommand(dto);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}