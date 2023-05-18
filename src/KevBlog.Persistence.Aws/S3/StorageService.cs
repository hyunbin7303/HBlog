using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;

namespace KevBlog.Persistence.Aws.S3
{
    public class AwsStorageService : IAwsStorageService
    {
        private readonly IAmazonS3 _client;
        public AwsStorageService(IConfiguration config)
        {
            string accessKey = config.GetSection("Aws")["AccessKey"].ToString();
            string secretKey = config.GetSection("Aws")["SecretKey"].ToString();
            _client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.CACentral1);
        }
        public async Task<bool> UploadFileAsync(Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
            TransferUtility utility = new TransferUtility(_client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
            {
                request.BucketName = bucketName; //no subdirectory just bucket name  
            }
            else
            {   // subdirectory and bucket name  
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;
            }
            request.Key = fileNameInS3; //file name up in S3  
            request.InputStream = localFilePath;
            await utility.UploadAsync(request); 
            return true; 
        }

        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            var data = await _client.ListBucketsAsync();
            var buckets = data.Buckets.Select(b => { return b.BucketName; });
            return buckets;
        }
    }
}
