namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

[Route("[controller]/[action]")]
public class AuthController : DispatcherControllerBase
{
    #region Constructors

    public AuthController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpPost,
     AllowAnonymous,
     ProducesResponseType(typeof(AuthDto.Result), 200)]
    public async Task<IActionResult> SignUp([FromBody] AuthDto.Request request)
    {
        var command = new SignUpCommand(userName: request.UserName,
                                        password: request.Password);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPost,
     AllowAnonymous,
     ProducesResponseType(typeof(AuthDto.Result), 200)]
    public async Task<ActionResult> SignIn([FromBody] AuthDto.Request request)
    {
        var command = new SignInCommand(userName: request.UserName,
                                        password: request.Password);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPost,
     Authorize,
     ProducesResponseType(typeof(AuthDto.Result), 200)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new RefreshTokenCommand(userId: currentUserId,
                                              refreshToken: request.RefreshToken);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}