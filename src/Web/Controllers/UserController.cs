//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using LMS.Application.Users.Commands.CreateUser;
//using LMS.Application.Users.Commands.DeleteUser;
//using LMS.Application.Users.Commands.LanguageUpdate;
//using LMS.Application.Users.Commands.UpdateUser;
//using LMS.Application.Users.Commands.UserProfile;
//using LMS.Application.Users.Queries.GetUsersWithPagination;

//namespace LMS.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
//public class UserController : ControllerBase
//{
//    private readonly ISender _sender;

//    public UserController(ISender sender)
//    {
//        _sender = sender;
//    }

//    [HttpGet("get")]
//    public async Task<IActionResult> GetUsers([FromQuery] GetUsersWithPaginationQuery query)
//    {
//        var result = await _sender.Send(query);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    [HttpPost("create")]
//    public async Task<IActionResult> CreateUser(CreateUserCommand command)
//    {
//        var result = await _sender.Send(command);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    [HttpPut("update")]
//    public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
//    {
//        var result = await _sender.Send(command);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    [HttpGet("get-rights")]
//    public async Task<IActionResult> GetUserRights()
//    {
//        var result = await _sender.Send(new GetUserRightsQuery());
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpDelete("delete/{userId}")]
//    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
//    {
//        var command = new UserDeleteCommand { UserId = userId };
//        var result = await _sender.Send(command);

//        if (result.Status)
//            return Ok(result);
//        else
//            return BadRequest(result);
//    }

//    [HttpGet("profile")]
//    public async Task<IActionResult> UserProfile()
//    {
//        var result = await _sender.Send(new UserProfileGetQuery());
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPut("language")]
//    public async Task<IActionResult> LanguageUpdate(LanguageUpdateCommand command)
//    {
//        var result = await _sender.Send(command);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }
//}
