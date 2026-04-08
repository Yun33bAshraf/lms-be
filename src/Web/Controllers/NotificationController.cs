//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Project.Application.Dashboard.Queries;
//using Project.Application.Notifications;

//namespace Project.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
//[ApiExplorerSettings(IgnoreApi = false)]
//public class NotificationController : ControllerBase
//{
//    private readonly ISender _sender;

//    public NotificationController(ISender sender)
//    {
//        _sender = sender;
//    }

//    [HttpGet("get")]
//    public async Task<IActionResult> GetNotification()
//    {
//        var result = await _sender.Send(new NotificationsGetQuery());
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPost("is-read")]
//    public async Task<IActionResult> MarkNotificationsAsRead()
//    {
//        var result = await _sender.Send(new NotificationIsReadCommand());
//        return result.Status ? Ok(result) : BadRequest(result);
//    }
//}
