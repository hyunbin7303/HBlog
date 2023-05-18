namespace KevBlog.Persistence.Aws.S3
{
    public interface IAwsStorageService
    {
        Task<IEnumerable<string>> GetAllBucketAsync();
        Task<bool> UploadFileAsync(Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3);
    }
}
