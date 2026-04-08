namespace LMS.Application.Common.Models;

public class FileDto
{
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public int FileFolderId { get; set; }
    public byte FileSize { get; set; }
    public string? FileUrl { get; set; }
    public int FileExtensionId { get; set; } //enum
    public int FileStoreId { get; set; }
}
