﻿namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

[Route("[controller]/[action]")]
public class AuthController : DispatcherControllerBase
{
    #region Properties

    private readonly IHttpContextAccessor httpCtx;

    #endregion

    #region Constructors

    public AuthController(IDispatcher dispatcher, IHttpContextAccessor httpCtx) : base(dispatcher)
    {
        this.httpCtx = httpCtx;
    }

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
        var command = new RefreshTokenCommand(HttpContext.User.ToUserDto().Id, request.RefreshToken);
        await Dispatcher.PushAsync(command);

        return Ok(command.Result);
    }
}