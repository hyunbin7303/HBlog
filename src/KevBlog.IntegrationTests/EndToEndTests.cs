using KevBlog.Persistence.Aws;
using KevBlog.Persistence.Aws.S3;
using Microsoft.Extensions.Options;

namespace KevBlog.IntegrationTests
{

    public class EndToEndTests : TestBase
    {
        private readonly IAwsStorageService awsStorageService;
        private IOptions<AwsSettings> _awsIOptions;
        public EndToEndTests()
        {
            awsStorageService = new AwsStorageService(_awsIOptions);
        }

        [Fact]
        public async Task UploadFileAsync_ExistingFile_ReturnTrue()
        {
            var bucketName = "kevblogbuc ket";
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"Names.txt");
            using FileStream fileStream = new FileStream(path, FileMode.Open);

            // Assert
            var result = await awsStorageService.UploadFileAsync(fileStream, bucketName, "IntegrationTest", "TestingFile.txt");

            Assert.Equal(true, result);
        }


        [Fact]
        public async Task DeleteAsync_ExistingFile_RemovedSuccessfully()
        {
            string bucketName = "kevblogbucket";
            string fileName = "Screen Shot 2023-05-17 at 5.17.38 PM.png";

            var result = await awsStorageService.DeleteAsync(bucketName, fileName);

            Assert.Equal(true, result);
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