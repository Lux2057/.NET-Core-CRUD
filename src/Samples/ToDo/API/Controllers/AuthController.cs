namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Samples.ToDo.Shared;

#endregion

[Route("[controller]/[action]")]
public class AuthController : DispatcherControllerBase
{
    #region Constructors

    public AuthController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpPost,
     AllowAnonymous,
     Route("~/" + ApiRoutesConst.SignUp),
     ProducesResponseType(typeof(AuthResultDto), 200)]
    public async Task<IActionResult> SignUp([FromBody] AuthRequest authRequest)
    {
        var command = new SignUpCommand(userName: authRequest.UserName,
                                        password: authRequest.Password);

        await Dispatcher.PushAsync(command);

        return Ok(command.AuthResultDto);
    }

    [HttpPost,
     AllowAnonymous,
     Route("~/" + ApiRoutesConst.SignIn),
     ProducesResponseType(typeof(AuthResultDto), 200)]
    public async Task<ActionResult> SignIn([FromBody] AuthRequest authRequest)
    {
        var command = new SignInCommand(userName: authRequest.UserName,
                                        password: authRequest.Password);

        await Dispatcher.PushAsync(command);

        return Ok(command.AuthResultDto);
    }

    [HttpPost,
     Authorize,
     Route("~/" + ApiRoutesConst.RefreshToken),
     ProducesResponseType(typeof(AuthResultDto), 200)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new RefreshTokenCommand(userId: currentUserId,
                                              refreshToken: request.RefreshToken);

        await Dispatcher.PushAsync(command);

        return Ok(command.AuthResultDto);
    }
}