using System.ComponentModel;

namespace LMS.Domain.Enums;

public enum FileFolder
{
    [Description("Ticket Attachments")]
    TicketAttachments = 1,
    [Description("Ticket Comment Attachments")]
    TicketCommentAttachments = 2,
    [Description("Asset Attachments")]
    AssetAttachments = 3,
}
