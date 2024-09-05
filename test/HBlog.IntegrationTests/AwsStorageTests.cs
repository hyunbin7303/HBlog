using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Repositories;
using HBlog.Infrastructure.Services;
using HBlog.IntegrationTests.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace HBlog.IntegrationTests
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

        [Test]
        public async Task UploadFileAsync_ExistingFile_ReturnTrue()
        {
            var result = await UploadTempFileInS3();

            Assert.That(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingFile_RemovedSuccessfully()
        {
            await UploadTempFileInS3();
            string bucketName = "kevblogbucket";

            var result = await awsStorageService.DeleteAsync(bucketName, "IntegrationTest/TestingFile.txt");

            Assert.That(result);
        }
        [Fact]
        public async Task GivenNotAuthorized_WhenIsAuthorized_ThenReturnFalse()
        {
            string bucketName = "kevblogbucket";

            var result = await awsStorageService.IsAuthorized(bucketName, 0);
            
            Assert.That(result, Is.False);
        }

        [Fact]
        public async Task GetFileTesting()
        {
            string fileName = "Screen Shot 2023-05-17 at 5.17.38 PM.png";
         
            var result = await awsStorageService.DownloadFileAsync("", fileName);

            Assert.That(result, Is.Not.Null);
        }

        [Fact]
        public async Task IsFileExists_PassExistingItems_ReturnTrue()
        {
            string fileName = "Screen Shot 2023-05-17 at 5.17.38 PM.png";

            var result = await awsStorageService.IsFileExists(fileName, string.Empty);

            Assert.That(result);
        }

        [Fact]
        public async Task IsFileExists_NonExisting_ReturnFalse()
        {
            string fileName = "TESTING";

            var result = await awsStorageService.IsFileExists(fileName, string.Empty);

            Assert.That(result, Is.False);
        }
    }
}