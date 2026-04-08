//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Project.Application.Dashboard.Queries;

//namespace Project.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
////[ApiExplorerSettings(IgnoreApi = true)]
//public class DashboardController(ISender sender) : ControllerBase
//{
//    [HttpGet("get")]
//    public async Task<IActionResult> GetDashboard([FromQuery] DashboardGetQuery query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }
//}
