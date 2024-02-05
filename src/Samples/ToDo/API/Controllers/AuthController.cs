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
     ProducesResponseType(typeof(AuthResultDto), 200)]
    public async Task<IActionResult> SignUp([FromBody] AuthRequest request)
    {
        var command = new SignUpCommand(request.UserName, request.Password);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPost,
     AllowAnonymous,
     ProducesResponseType(typeof(AuthResultDto), 200)]
    public async Task<ActionResult> SignIn([FromBody] AuthRequest request)
    {
        var command = new SignInCommand(request.UserName, request.Password);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }

    [HttpPost,
     Authorize,
     ProducesResponseType(typeof(AuthResultDto), 200)]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var currentUser = await Dispatcher.QueryAsync(new GetCurrentUserOrDefaultQuery());

        var command = new RefreshTokenCommand(currentUser?.Id ?? 0, request.RefreshToken);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}