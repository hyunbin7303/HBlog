using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using KevBlog.Infrastructure.Repositories;
using KevBlog.Persistence.Aws;
using KevBlog.Persistence.Aws.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace KevBlog.IntegrationTests
{

    public class AwsStorageTests : IntegrationTestBase
    {
        private readonly IAwsStorageService awsStorageService;
        private readonly IUserRepository userRepository;
        private IOptions<AwsSettings> _awsIOptions;
        private Mock<IFileDataRepository> _mockDataRepository;
        private Mock<IFileStorageRepository> _mockFileStorageRepository;
        public AwsStorageTests()
        {
            _mockFileStorageRepository = new();
            _mockDataRepository = new();
            var myOptions = new AwsSettings();
            _config.GetSection("AwsSettings").Bind(myOptions);
            _awsIOptions = Options.Create(myOptions);

            userRepository = new UserRepository(_dataContext);
            awsStorageService = new AwsStorageService(_awsIOptions, _mockFileStorageRepository.Object, _mockDataRepository.Object, userRepository);
        }

        private async Task<bool> UploadTempFileInS3()
        {
            var bucketName = "kevblogbucket";
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"TestingFile.txt");
            using FileStream fileStream = new FileStream(path, FileMode.Open);

            var result = await awsStorageService.UploadFileAsync(fileStream, bucketName,  "IntegrationTest/TestingFile.txt");
            return result;
        }

        [Fact]
        public async Task UploadFileAsync_ExistingFile_ReturnTrue()
        {
            var result = await UploadTempFileInS3();

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingFile_RemovedSuccessfully()
        {
            await UploadTempFileInS3();
            string bucketName = "kevblogbucket";

            var result = await awsStorageService.DeleteAsync(bucketName, "IntegrationTest/TestingFile.txt");

            Assert.True(result);
        }
        [Fact]
        public async Task GivenNotAuthorized_WhenIsAuthorized_ThenReturnFalse()
        {
            string bucketName = "kevblogbucket";

            var result = await awsStorageService.IsAuthorized(bucketName, 0);
            
            Assert.False(result);
        }

        [Fact]
        public async Task GetFileTesting()
        {
            string fileName = "Screen Shot 2023-05-17 at 5.17.38 PM.png";
         
            var result = await awsStorageService.DownloadFileAsync("", fileName);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task IsFileExists_PassExistingItems_ReturnTrue()
        {
            string fileName = "Screen Shot 2023-05-17 at 5.17.38 PM.png";

            var result = await awsStorageService.IsFileExists(fileName, string.Empty);

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task IsFileExists_NonExisting_ReturnFalse()
        {
            string fileName = "TESTING";

            var result = await awsStorageService.IsFileExists(fileName, string.Empty);

            Assert.Equal(false, result);
        }
    }
}