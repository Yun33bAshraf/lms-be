//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using LMS.Application.Roles.Commands;
//using LMS.Application.Roles.Queries.GetRolesWithPagination;
//using LMS.Application.Roles.Rights.Queries;

//namespace LMS.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
////[ApiExplorerSettings(IgnoreApi = true)]
//public class RoleRightsController : ControllerBase
//{
//    private readonly ISender _sender;

//    public RoleRightsController(ISender sender)
//    {
//        _sender = sender;
//    }

//    [HttpGet("roles-get")]
//    public async Task<IActionResult> GetRoleRight([FromQuery] GetRolesWithPaginationQuery query)
//    {
//        var result = await _sender.Send(query);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    [HttpPost("roles-create")]
//    public async Task<IActionResult> CreateRoleRights(CreateRoleRightsCommand request)
//    {
//        // TODO: Look up how can I get the current user id
//        // request.CurrentUserId = CurrentUserId ?? 0;
//        var result = await _sender.Send(request);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    [HttpPut("roles-update")]
//    public async Task<IActionResult> UpdateRoleRights(UpdateRoleRightsCommand request)
//    {
//        // TODO: Look up how can I get the current user id
//        // request.CurrentUserId = CurrentUserId ?? 0;
//        var result = await _sender.Send(request);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    [HttpDelete("delete/{roleId}")]
//    public async Task<IActionResult> DeleteRoleRight([FromRoute] int roleId)
//    {
//        var command = new DeleteRoleRightCommand { RoleId = roleId };
//        var result = await _sender.Send(command);

//        if (result.Status)
//            return Ok(result);
//        else
//            return BadRequest(result);
//    }

//    #region RIGHTS

//    [HttpGet("rights-get")]
//    public async Task<IActionResult> GetRights([FromQuery] GetRightsQuery query)
//    {
//        var result = await _sender.Send(query);
//        if (result.Status) return Ok(result); else return BadRequest(result);
//    }

//    #endregion RIGHTS
//}
