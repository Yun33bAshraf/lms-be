//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Project.Application.Auth.Command.ChangePassword;
//using Project.Application.Auth.Command.CompleteForgetPassword;
//using Project.Application.Auth.Command.ForgetPassword;
//using Project.Application.Auth.Command.Login;
//using Project.Application.Auth.Command.Register;
//using Project.Application.Users.Commands.CompleteRegistration;

//namespace Project.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//public class AuthController : ControllerBase
//{
//    private readonly ISender _sender;

//    public AuthController(ISender sender)
//    {
//        _sender = sender;
//    }

//    [AllowAnonymous]
//    [HttpPost("register")]
//    public async Task<IActionResult> CreateUser(RegisterCommand command)
//    {
//        var result = await _sender.Send(command);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    [AllowAnonymous]
//    [HttpPost("login")]
//    public async Task<IResult> Login(ISender sender, LoginCommand query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
//    }

//    //[Authorize]
//    [HttpPost("change-password")]
//    public async Task<IResult> ChangePassword([FromBody] ChangePasswordCommand query)
//    {
//        var result = await _sender.Send(query);
//        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
//    }

//    [AllowAnonymous]
//    [HttpPost("forget-password")]
//    public async Task<IResult> ForgetPassword(ISender sender, ForgetPasswordCommand query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
//    }

//    [AllowAnonymous]
//    [HttpPost("reset-password")]
//    public async Task<IResult> CompletePasswordReset(ISender sender, CompleteForgetPasswordCommand query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
//    }

//    [AllowAnonymous]
//    [HttpPost("complete-registration")]
//    public async Task<IResult> CompleteRegistration(ISender sender, CompleteRegistrationCommand query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Results.Ok(result) : Results.BadRequest(result);
//    }
//}
