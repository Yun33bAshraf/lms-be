//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using LMS.Application.TicketComments.Commands;
//using LMS.Application.TicketComments.Commands.Attachments;
//using LMS.Application.Tickets.Commands;
//using LMS.Application.Tickets.Commands.Attachments;
//using LMS.Application.Tickets.Queries;

//namespace LMS.Web.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//[Authorize]
//public class TicketController(ISender sender) : ControllerBase
//{
//    [HttpGet("get")]
//    public async Task<IActionResult> GetAllTickets([FromQuery] GetTicketsWithPaginationQuery query)
//    {
//        var result = await sender.Send(query);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPost("create")]
//    public async Task<IActionResult> CreateTicket(CreateTicketCommand request)
//    {
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPut("update")]
//    public async Task<IActionResult> UpdateTicket(UpdateTicketCommand request)
//    {
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPut("update-status")]
//    public async Task<IActionResult> UpdateTicketStatus(UpdateTicketStatusCommand request)
//    {
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpDelete("delete/{ticketId}")]
//    public async Task<IActionResult> DeleteTicket([FromRoute] int ticketId)
//    {
//        var command = new TicketDeleteCommand { TicketId = ticketId };
//        var result = await sender.Send(command);

//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPost("{ticketId}/attachments")]
//    public async Task<IActionResult> UploadAttachments(
//        int ticketId,
//        [FromForm] List<IFormFile> files)
//    {
//        if (files == null || files.Count == 0)
//            return BadRequest("No files were uploaded.");

//        if (files.Any(f => f.Length == 0))
//            return BadRequest("One or more files are empty.");

//        var command = new TicketAttachmentUploadCommand
//        {
//            TicketId = ticketId,
//            Files = files
//        };

//        var result = await sender.Send(command);

//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    #region TICKET COMMENT

//    [HttpPost("add-comment")]
//    //[ApiExplorerSettings(IgnoreApi = true)]
//    public async Task<IActionResult> CreateTicketComment(CreateTicketCommentCommand request)
//    {
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPut("update-comment")]
//    //[ApiExplorerSettings(IgnoreApi = true)]
//    public async Task<IActionResult> UpdateTicketComment(UpdateTicketCommentCommand request)
//    {
//        var result = await sender.Send(request);
//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    [HttpPost("{ticketId}/comments/{commentId}/attachments")]
//    public async Task<IActionResult> UploadCommentAttachments(
//    int ticketId,
//    int commentId,
//    [FromForm] List<IFormFile> files)
//    {
//        if (files == null || files.Count == 0)
//            return BadRequest("No files uploaded.");

//        var command = new TicketCommentAttachmentUploadCommand
//        {
//            TicketId = ticketId,
//            TicketCommentId = commentId,
//            Files = files
//        };

//        var result = await sender.Send(command);

//        return result.Status ? Ok(result) : BadRequest(result);
//    }

//    #endregion TICKET COMMENT
//}
