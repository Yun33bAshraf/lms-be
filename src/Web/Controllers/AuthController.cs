using LMS.Application.Auth.Login;
using LMS.Application.Auth.RefreshToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
    //[AllowAnonymous]
    //[HttpPost("register")]
    //public async Task<IActionResult> CreateUser(RegisterCommand command)
    //{
    //    var result = await _sender.Send(command);
    //    if (result.Status) return Ok(result); else return BadRequest(result);
    //}

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IResult> Login(LoginRequest query)
    {
        var result = await sender.Send(query);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IResult> RefreshToken(RefreshTokenRequest query)
    {
        var result = await sender.Send(query);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    ////[Authorize]
    //[HttpPost("change-password")]
    //public async Task<IResult> ChangePassword([FromBody] ChangePasswordCommand query)
    //{
    //    var result = await _sender.Send(query);
    //    return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    //}

    //[AllowAnonymous]
    //[HttpPost("forget-password")]
    //public async Task<IResult> ForgetPassword(ISender sender, ForgetPasswordCommand query)
    //{
    //    var result = await sender.Send(query);
    //    return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    //}

    //[AllowAnonymous]
    //[HttpPost("reset-password")]
    //public async Task<IResult> CompletePasswordReset(ISender sender, CompleteForgetPasswordCommand query)
    //{
    //    var result = await sender.Send(query);
    //    return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    //}

    //[AllowAnonymous]
    //[HttpPost("complete-registration")]
    //public async Task<IResult> CompleteRegistration(ISender sender, CompleteRegistrationCommand query)
    //{
    //    var result = await sender.Send(query);
    //    return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    //}
}
