using LMS.Application.Common.Models;
using LMS.Domain.Enums;

namespace LMS.Application.Common.Interfaces;

public interface IFileUploadRepository
{
    Task<FileDto> UploadAndStoreFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        byte fileSize,
        int userId,
        FileFolder fileFolder);

    string GetFullFileUrl(string relativePath);

    string FormatFileSize(long bytes);
}
