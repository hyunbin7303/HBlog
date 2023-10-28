namespace KevBlog.Persistence.Aws.S3
{
    public interface IAwsStorageService
    {
        Task<bool> CreateBucketAsync(string bucketName, int userId);
        Task<bool> UploadFileAsync(Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3);
        Task<byte[]> DownloadFileAsync(string bucket, string file);
        Task<bool> DeleteAsync(string bucket, string fileName, string versionId = "");
        Task<bool> IsFileExists(string bucket, string fileName, string versionId = "");
    }
}
