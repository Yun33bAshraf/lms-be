using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using LMS.Domain.Common;
using LMS.Domain.Entities;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Domain.Enums;

namespace LMS.Infrastructure.Repositories;

public class FileUploadRepository(IWebHostEnvironment webHostEnvironment, IApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor) : IFileUploadRepository
{
    private readonly string _filesRootPath = Path.Combine(webHostEnvironment.WebRootPath, "Files");

    public async Task<FileDto> UploadAndStoreFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        byte fileSize,
        int userId,
        FileFolder fileFolder)
    {
        // 1. Create subfolder path inside Files root
        var subFolder = fileFolder.ToString();
        //var relativeSubPath = Path.Combine(subFolder).Replace("\\", "/");
        var fullSubPath = Path.Combine(_filesRootPath, subFolder);

        Directory.CreateDirectory(fullSubPath);

        // 2. Generate safe unique filename
        var safeFileName = GenerateSafeFileName(fileName);

        // 3. Full paths
        var relativePath = Path.Combine("Files", subFolder, safeFileName).Replace("\\", "/");
        var fullPhysicalPath = Path.Combine(_filesRootPath, subFolder, safeFileName);

        // 4. Save file to disk
        await using (var fileOutputStream = new FileStream(fullPhysicalPath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.CopyToAsync(fileOutputStream);
        }

        // 5. Determine file extension
        var extension = GetFileExtensionFromName(fileName)
                       ?? GetFileExtensionFromContentType(contentType)
                       ?? throw new InvalidOperationException("Unsupported file type.");

        // 6. Build FileDto
        var fileDto = new FileDto
        {
            FileName = fileName,
            FilePath = relativePath, // e.g., Files/Ticket Attachments/abc123_photo.jpg
            ContentType = contentType,
            FileSize = fileSize,
            FileFolderId = (int)fileFolder,
            FileExtensionId = (int)extension,
            FileUrl = $"/{relativePath}" // Direct public URL: https://yoursite.com/Files/Ticket Attachments/...
        };

        // 7. Save metadata to database
        var fileStore = new FileStore
        {
            FileName = fileName,
            SafeFileName = safeFileName,
            ContentType = contentType,
            FileSize = fileSize,
            FileFolderId = (int)fileFolder,
            FileExtensionId = (int)extension,
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            ModifiedBy = userId,
            ModifiedAt = DateTime.UtcNow
        };

        dbContext.FileStore.Add(fileStore);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        // 8. Enrich DTO with DB ID
        fileDto.FileStoreId = fileStore.Id;

        return fileDto;
    }

    private string GenerateSafeFileName(string originalFileName)
    {
        var extension = Path.GetExtension(originalFileName);
        var nameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);

        // Sanitize: only alphanumeric, hyphen, underscore
        var safeName = Regex.Replace(nameWithoutExt, @"[^a-zA-Z0-9\-_]", "_");

        if (string.IsNullOrWhiteSpace(safeName))
            safeName = "file";

        // Unique with GUID
        return $"{Guid.NewGuid():N}_{safeName}{extension.ToLowerInvariant()}";
    }

    private static FileExtension? GetFileExtensionFromName(string fileName)
    {
        var ext = Path.GetExtension(fileName)?.ToLowerInvariant();
        return ext switch
        {
            ".pdf" => FileExtension.PDF,
            ".docx" => FileExtension.DOCX,
            ".png" => FileExtension.PNG,
            ".jpg" or ".jpeg" => FileExtension.JPG,
            //".gif" => FileExtension.GIF,
            //".txt" => FileExtension.TXT,
            _ => null
        };
    }

    private static FileExtension? GetFileExtensionFromContentType(string contentType)
    {
        return contentType.ToLowerInvariant() switch
        {
            "application/pdf" => FileExtension.PDF,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => FileExtension.DOCX,
            "image/png" => FileExtension.PNG,
            "image/jpeg" => FileExtension.JPG,
            //"image/gif" => FileExtension.GIF,
            //"text/plain" => FileExtension.TXT,
            _ => null
        };
    }

    public string GetFullFileUrl(string relativePath)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return relativePath; // Fallback

        // Build base URL: scheme + host + base path
        var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

        // Ensure no double slash
        return $"{baseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
    }

    public string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB" };
        int counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        return $"{number:n1} {suffixes[counter]}";
    }
}
