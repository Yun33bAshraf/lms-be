//using Amazon.S3;
//using Amazon.S3.Model;
//using Amazon.S3.Transfer;
//using Project.Application.Common.Models;
//using Project.Application.Services;
//using Project.Domain.Common;
//using Project.Domain.Enums;

//namespace Project.Infrastructure.Repositories;

//public class S3FileService(IAmazonS3 amazonS3) : IS3FileService
//{
//    public async Task<(FileDto file, string url)> UploadFileAsync(Stream fileStream, FileDto fileDto, FileFolder fileFolder, string awsFolder)
//    {
//        try
//        {
//            var fileUrl = string.Empty;
//            var filePath = $"{fileFolder.GetDescription()}/{Path.GetFileName(fileDto.FileName)}";

//            fileDto.FilePath = filePath;
//            //fileDto.AWSBucketName = _bucketName;
//            var tags = GenerateTagsFromFileDto(fileDto);

//            var uploadRequest = new TransferUtilityUploadRequest
//            {
//                InputStream = fileStream,
//                Key = filePath,
//                BucketName = awsFolder,
//                ContentType = fileDto.ContentType,
//                //CannedACL = S3CannedACL. // or Private
//                TagSet = tags
//            };

//            var fileTransferUtility = new TransferUtility(amazonS3);
//            await fileTransferUtility.UploadAsync(uploadRequest);
//            fileUrl = await GetFileUrlAsync(awsFolder, filePath);

//            return (fileDto, fileUrl);
//        }
//        catch (Exception ex)
//        {
//            // TODO: log
//            throw new Exception("Error uploading file to S3", ex);
//        }
//    }

//    public async Task<string> GetFileUrlAsync(string awsFolder, string filepath, int expiresInMin = 15)
//    {
//        try
//        {
//            var request = new GetPreSignedUrlRequest
//            {
//                BucketName = awsFolder,
//                Key = filepath,
//                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(expiresInMin)),
//                //Verb = HttpVerb.GET
//            };

//            return amazonS3.GetPreSignedURL(request);
//        }
//        catch (Exception)
//        {
//            return string.Empty;
//        }
//    }

//    private List<Tag> GenerateTagsFromFileDto(FileDto fileDto)
//    {
//        var tags = new List<Tag>();

//        var properties = typeof(FileDto).GetProperties();
//        foreach (var prop in properties)
//        {
//            var key = prop.Name;
//            var value = prop.GetValue(fileDto)?.ToString() ?? string.Empty;

//            tags.Add(new Tag { Key = SanitizeForTag(key), Value = SanitizeForTag(value) });
//        }

//        return tags;
//    }

//    private string SanitizeForTag(string input)
//    {
//        if (string.IsNullOrEmpty(input)) return string.Empty;

//        // Replace or remove invalid characters
//        var allowed = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ._:/+=@-";
//        var result = new string(input.Where(c => allowed.Contains(c)).ToArray());

//        return result.Length > 256 ? result.Substring(0, 256) : result;
//    }

//}
