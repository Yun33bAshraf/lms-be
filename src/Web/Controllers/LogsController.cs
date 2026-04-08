//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using LMS.Application.SystemLogs.Command.Delete;
//using LMS.Application.SystemLogs.Query.Get;

//namespace LMS.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
////[ApiExplorerSettings(IgnoreApi = true)]
//public class LogsController(ISender sender) : ControllerBase
//{
//    [HttpGet("get")]
//    public async Task<IActionResult> GetLogs([FromQuery] GetLogsQuery query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpDelete("clear-logs")]
//    public async Task<IActionResult> ClearLogs([FromQuery] string level)
//    {
//        var command = new LogsDeleteCommand { Level = level };

//        var result = await sender.Send(command);

//        return result.Status
//            ? Ok(result)
//            : BadRequest(result);
//    }
//}
