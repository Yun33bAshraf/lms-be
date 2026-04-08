namespace LMS.Application.Common.Models;

public class TicketDto
{
    public int TicketId { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? TitleAR { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? DescriptionAR { get; set; } = string.Empty;
    public string? TicketNo { get; set; }
    //public string? TicketNoAR { get; set; }
    public int StatusId { get; set; }
    public string? Status { get; set; }
    public int PriorityId { get; set; }
    public string? Priority { get; set; }
    public int Index { get; set; }
    //public int? AssignedToId { get; set; }
    //public string? AssignedTo { get; set; }
    public int CategoryId { get; set; }
    public string? Category { get; set; }
    public int? User { get; set; }
    public DateTimeOffset Created { get; set; }
    public int? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public int? LastModifiedBy { get; set; }
    public List<TicketFilesDto>? TicketFiles { get; set; } = [];
    public List<TicketAssignmentDto>? TicketAssignments { get; set; } = [];
    public List<TicketAssetDto>? TicketAssets { get; set; } = [];
    public List<TicketCommentDto>? TicketComments { get; set; } = [];
    public List<TicketCategoryDto>? TicketCategories { get; set; } = [];
}

public class TicketFilesDto
{
    public int? TicketId { get; set; }
    public int? TicketFileId { get; set; }
    public int? CategoryId { get; set; }
    public string? Category { get; set; }
    public int? FileStoreId { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string? ContentType { get; set; } = string.Empty;
    public int? FileFolderId { get; set; }
    public string? FileFolder { get; set; }
    public byte? FileSize { get; set; }
    public int? FileExtensionId { get; set; }
    public string? FileExtension { get; set; }
}

public class TicketAssetDto
{
    public int TicketId { get; set; }
    public int AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameAr { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public string? AssetNumber { get; set; }
    public DateTime AssignedDate { get; set; }
    public string? Condition { get; set; }
    public string? ConditionAr { get; set; }
}

public class TicketAssignmentDto
{
    public int TicketAssignmentId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public DateTime AssignedAt { get; set; }
    public string? Note { get; set; }
    public string? NoteAr { get; set; }
}

public class TicketCommentDto
{
    public int TicketCommentId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public string? Comment { get; set; }
    public string? CommentAr { get; set; }
    public DateTimeOffset Created { get; set; }
    public List<TicketCommentFileDto> CommentFiles { get; set; } = [];
}

public class TicketCommentFileDto
{
    public int TicketCommentId { get; set; }
    public int FileStoreId { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public int FileFolderId { get; set; }
    public string? FileFolder { get; set; }
    public byte FileSize { get; set; }
    public int FileExtensionId { get; set; }
    public string? FileExtension { get; set; }
}

public class TicketCategoryDto
{
    public int TicketCategoryId { get; set; }
    public int TicketId { get; set; }
    public int CategoryId { get; set; }
    public string? Category { get; set; }
    public string? CategoryAr { get; set; }
}
