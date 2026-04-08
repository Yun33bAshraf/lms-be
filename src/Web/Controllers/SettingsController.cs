//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Project.Application.EntityType.Category.Commands;
//using Project.Application.EntityType.Category.Queries;
//using Project.Application.EntityType.Queries.GetEntityTypeCategoriesWithPagination;

//namespace Project.Web.Controllers;


//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
////[ApiExplorerSettings(IgnoreApi = true)]
//public class SettingsController(ISender sender) : ControllerBase
//{
//    [HttpGet("entity-type-get")]
//    public async Task<IActionResult> GetEntityTypeCategories([FromQuery] GetEntityTypeCategoriesWithPaginationQuery query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpGet("category-get")]
//    public async Task<IActionResult> CategoryGet([FromQuery] CategoryGetQuery query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPost("category-create")]
//    public async Task<IActionResult> CategoryCreate(CategoryCreateCommand command)
//    {
//        var result = await sender.Send(command);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPut("category-update")]
//    public async Task<IActionResult> CategoryUpdate(CategoryUpdateCommand command)
//    {
//        var result = await sender.Send(command);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }
//}
