using LMS.Application.Tenant.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
//[ApiExplorerSettings(IgnoreApi = true)]
public class TenantController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> TenantCreate(TenantCreateRequest request)
    {
        var result = await sender.Send(request);
        return result.Status ? Ok(result) : BadRequest(result);
    }
}
