using KevBlog.Persistence.Aws.S3;
namespace KevBlog.IntegrationTests
{

    public class AwsTests : TestBase
    {
        private readonly IAwsStorageService awsStorageService;

        public AwsTests()
        {
            awsStorageService = new AwsStorageService(_config);
        }

        [Fact]
        public async Task UploadFileAsync_ExistingFile_ReturnTrue()
        {
            var bucketName = "kevblogbucket";
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"Names.txt");
            using FileStream fileStream = new FileStream(path, FileMode.Open);

            // Assert
            var result = await awsStorageService.UploadFileAsync(fileStream, bucketName, "Testing", "TestingAPP.txt");

            Assert.Equal(true, result);
        }
    }
}