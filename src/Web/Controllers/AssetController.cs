//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using LMS.Application.Assets.Command.Attachment;
//using LMS.Application.Assets.Command.Create;
//using LMS.Application.Assets.Command.Repairs.Create;
//using LMS.Application.Assets.Command.Repairs.Get;
//using LMS.Application.Assets.Command.Repairs.Update;
//using LMS.Application.Assets.Command.Update;
//using LMS.Application.Assets.Queries;

//namespace LMS.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
////[ApiExplorerSettings(IgnoreApi = true)]
//public class AssetController(ISender sender) : ControllerBase
//{
//    [HttpGet("get")]
//    public async Task<IActionResult> GetAllTickets([FromQuery] AssetGetQuery query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPost("create")]
//    public async Task<IActionResult> AssetCreate(AssetCreateCommand request)
//    {
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPut("update")]
//    public async Task<IActionResult> AssetUpdate(AssetUpdateCommand request)
//    {
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPost("{assetId}/attachments")]
//    public async Task<IActionResult> UploadAttachments(
//        int assetId,
//        [FromForm] List<IFormFile> files)
//    {
//        var command = new AssetAttachmentCreateCommand
//        {
//            AssetId = assetId,
//            Files = files
//        };

//        var result = await sender.Send(command);

//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    #region REPAIRS

//    [HttpPost("{assetId}/repairs")]
//    public async Task<IActionResult> CreateRepair(
//        int assetId,
//        AssetRepairCreateCommand request)
//    {
//        request.AssetId = assetId;
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPut("{assetId}/repairs/{repairId}")]
//    public async Task<IActionResult> UpdateRepair(
//        int assetId,
//        int repairId,
//        AssetRepairUpdateCommand request)
//    {
//        request.AssetId = assetId;
//        request.RepairId = repairId;
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpGet("{assetId}/repairs")]
//    public async Task<IActionResult> GetAssetRepairs(int assetId, [FromQuery] AssetRepairGetQuery query)
//    {
//        query.AssetId = assetId;
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpGet("repairs")]
//    public async Task<IActionResult> GetAllRepairs([FromQuery] AssetRepairGetQuery query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    #endregion REPAIRS
//}
