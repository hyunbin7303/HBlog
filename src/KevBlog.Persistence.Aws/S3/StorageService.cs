using Amazon;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace KevBlog.Persistence.Aws.S3
{
    public class AwsStorageService : IAwsStorageService
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _client;
        public AwsStorageService(IConfiguration config)
        {
            string accessKey = config.GetSection("Aws")["AccessKey"].ToString();
            string secretKey = config.GetSection("Aws")["SecretKey"].ToString();
            _bucketName = config.GetSection("Aws")["BucketName"];
            _client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.CACentral1);
        }
        public async Task<bool> UploadFileAsync(Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
            TransferUtility utility = new TransferUtility(_client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            if (string.IsNullOrEmpty(subDirectoryInBucket))
                request.BucketName = bucketName;
            else
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;

            request.Key = fileNameInS3; //file name up in S3  
            request.InputStream = localFilePath;
            await utility.UploadAsync(request);
            return true;
        }
        public async Task<byte[]> DownloadFileAsync(string file)
        {
            MemoryStream ms = null;

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = file
                };

                using (var response = await _client.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }

                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

                return ms.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteAsync(string fileName, string versionId = "")
        {
            try
            {
                DeleteObjectRequest deleteObjectRequest = new()
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    //VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };
                await _client.DeleteObjectAsync(deleteObjectRequest);
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
                return false;
            }
        }

        public async Task<bool> IsFileExists(string fileName, string versionId)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    VersionId = !string.IsNullOrEmpty(versionId) ? versionId : null
                };

                var response = await _client.GetObjectMetadataAsync(request);

                return true;
            }
            catch (Exception ex)
            {

                if (ex.InnerException != null && ex.InnerException is HttpErrorResponseException awsEx)
                {
                    //if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                    //    return false;
                    //else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                    return false;
                }

                throw;
            }
        }

        public async Task<bool> CreateBucketAsync(string bucketName)
        {
            try
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true,
                };

                var response = await _client.PutBucketAsync(request);
                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error creating bucket: '{ex.Message}'");
                return false;
            }
        }
    }
}
