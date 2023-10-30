using Amazon;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Microsoft.Extensions.Options;
using System.Net;

namespace KevBlog.Persistence.Aws.S3
{
    public class AwsStorageService : IAwsStorageService
    {
        private readonly IAmazonS3 _client;
        private readonly IFileStorageRepository _fileStorageRepository;
        private readonly IFileDataRepository _dataRepository;
        private readonly IUserRepository _userRepository;
        public AwsStorageService(IOptions<AwsSettings> config, IFileStorageRepository fileStorageRepository, IFileDataRepository dataRepository, IUserRepository userRepository)
        {
            _client = new AmazonS3Client(config.Value.AccessKey, config.Value.SecretKey, RegionEndpoint.CACentral1);
            _dataRepository = dataRepository;
            _fileStorageRepository = fileStorageRepository;
            _userRepository = userRepository;
        }
        public async Task<bool> UploadFileAsync(Stream localFilePath, string bucketName, string key)
        {
            TransferUtility utility = new TransferUtility(_client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            request.BucketName = bucketName;
            request.Key = key;
            request.InputStream = localFilePath;
            await utility.UploadAsync(request);


            return true;
        }
        public async Task<byte[]> DownloadFileAsync(string bucket, string file)
        {
            MemoryStream ms = null;

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = bucket,
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
        public async Task<bool> DeleteAsync(string bucket, string fileName, string versionId = "")
        {
            try
            {
                DeleteObjectRequest deleteObjectRequest = new()
                {
                    BucketName = bucket,
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

        public async Task<bool> IsFileExists(string bucket, string fileName, string versionId)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = bucket,
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

        public async Task<bool> CreateBucketAsync(string bucketName, int userId)
        {
            if(bucketName == null) throw new ArgumentNullException(nameof(bucketName));

            var user = await _userRepository.GetUserByIdAsync(userId);
            try
            {
                var request = new PutBucketRequest
                {
                    BucketName = bucketName,
                    //UseClientRegion = true,
                };

                var response = await _client.PutBucketAsync(request);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                    return false;
                _fileStorageRepository.Add(new FileStorage { BucketName = bucketName, UserId = user.Id, IsPublic = true, StorageType = "" });
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error creating bucket: '{ex.Message}'");
                return false;
            }
        }
        public Task<bool> IsAuthorized(string bucketName, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
