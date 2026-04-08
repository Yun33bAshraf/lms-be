using Project.Domain.Common;

namespace Project.Domain.Entities;

public class FileStore : BaseAuditableEntity
{
    public string FileName { get; set; } = string.Empty;
    public string SafeFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public int FileFolderId { get; set; } //enum
    public byte FileSize { get; set; }
    public int FileExtensionId { get; set; } //enum
}
