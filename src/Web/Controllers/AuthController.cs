using LMS.Application.Auth.ChangePassword;
using LMS.Application.Auth.ForgotPassword;
using LMS.Application.Auth.Login;
using LMS.Application.Auth.Logout;
using LMS.Application.Auth.RefreshToken;
using LMS.Application.Auth.ResetPassword;
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

    [HttpPost("logout")]
    public async Task<IResult> Logout(LogoutRequest query)
    {
        var result = await sender.Send(query);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [HttpPost("change-password")]
    public async Task<IResult> ChangePassword(ChangePasswordRequest query)
    {
        var result = await sender.Send(query);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("forgot-password")]
    public async Task<IResult> ForgotPassword(ForgotPasswordRequest query)
    {
        var result = await sender.Send(query);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IResult> ResetPassword(ResetPasswordRequest query)
    {
        var result = await sender.Send(query);
        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    }

    //[AllowAnonymous]
    //[HttpPost("complete-registration")]
    //public async Task<IResult> CompleteRegistration(ISender sender, CompleteRegistrationCommand query)
    //{
    //    var result = await sender.Send(query);
    //    return result.Status ? Results.Ok(result) : Results.BadRequest(result);
    //}
}
