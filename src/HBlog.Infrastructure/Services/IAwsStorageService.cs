namespace HBlog.Infrastructure.Services;
public interface IAwsStorageService
{
    Task<bool> CreateBucketAsync(string bucketName, Guid userId);
    Task<bool> UploadFileAsync(Stream localFilePath, string bucketName, string key);
    Task<byte[]> DownloadFileAsync(string bucket, string file);
    Task<bool> DeleteAsync(string bucket, string fileName, string versionId = "");
    Task<bool> IsFileExists(string bucket, string fileName, string versionId = "");
}
