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
     Route("~/" + ApiRoutes.SignUp),
     ProducesResponseType(typeof(AuthInfoDto), 200)]
    public async Task<IActionResult> SignUp([FromBody] AuthRequest authRequest)
    {
        var command = new SignUpCommand(userName: authRequest.UserName,
                                        password: authRequest.Password);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPost,
     AllowAnonymous,
     Route("~/" + ApiRoutes.SignIn),
     ProducesResponseType(typeof(AuthInfoDto), 200)]
    public async Task<ActionResult> SignIn([FromBody] AuthRequest authRequest)
    {
        var command = new SignInCommand(userName: authRequest.UserName,
                                        password: authRequest.Password);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPost,
     AllowAnonymous,
     Route("~/" + ApiRoutes.RefreshToken),
     ProducesResponseType(typeof(AuthInfoDto), 200)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var currentUserId = await Dispatcher.QueryAsync(new GetCurrentUserIdOrDefaultQuery());

        var command = new RefreshTokenCommand(userId: currentUserId,
                                              refreshToken: request.RefreshToken);

        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}