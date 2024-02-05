namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

[Route("[controller]/[action]")]
public class UsersController : DispatcherControllerBase
{
    #region Constructors

    public UsersController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpPost, ProducesResponseType(typeof(AuthResultDto), 200)]
    public async Task<IActionResult> SignUp([FromBody] SignInDto dto)
    {
        var command = new CreateUserCommand(dto.UserName, dto.Password);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [Authorize, HttpDelete, ProducesResponseType(200)]
    public async Task<IActionResult> Delete([FromBody] int[] ids)
    {
        await Dispatcher.PushAsync(new MarkEntitiesAsDeletedCommand<UserEntity>(ids));

        return Ok();
    }

    [Authorize, HttpGet, ProducesResponseType(typeof(string), 200)]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var userName = await Dispatcher.QueryAsync(new GetUserNameQuery(id));

        return Ok(userName);
    }
}