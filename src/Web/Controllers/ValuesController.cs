//using Microsoft.AspNetCore.Mvc;
//using Project.Application.EntityType.Queries.GetEntityTypeCategoriesWithPagination;
//using Project.Application.EntityTypes.Queries.GetEntityTypesWithPagination;

//namespace Project.Web.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[ApiExplorerSettings(IgnoreApi = true)]
//public class ValuesController : ControllerBase
//{
//    private readonly ISender _sender;

//    public ValuesController(ISender sender)
//    {
//        _sender = sender;
//    }

//    [HttpPost("register")]
//    public async Task<IActionResult> Register()
//    {
//        var result = await _sender.Send(new GetEntityTypeCategoriesWithPaginationQuery()
//        {
//            EntityTypeId = 55
//        });

//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }
//}
