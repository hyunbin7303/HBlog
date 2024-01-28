using KevBlog.Application.Services;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
namespace KevBlog.UnitTests.Services
{
    public class StorageServiceTest : TestBase
    {
        private IAwsStorageService _storageService;
        private IOptions<AwsSettings> _options;
        private Mock<IFileStorageRepository> _mockStorageRepository;
        private Mock<IFileDataRepository> _mockDataRepository;
        private Mock<IUserRepository> _mockUserRepository;
        public StorageServiceTest()
        {
            _mockStorageRepository = new();
            _mockUserRepository = new();
            _mockDataRepository = new();
            _storageService = new AwsStorageService(_options, _mockStorageRepository.Object, _mockDataRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task GivenEmptyName_CreateBucket_ThenReturnError()
        {
            var result = await _storageService.CreateBucketAsync(string.Empty, 0);

            Assert.False(result);
        }

        //[Fact]
        //public async Task Given_When_Then()
        //{
        //    var result = await _storageService.CreateBucketAsync("NewBucket", 0);
        //}
    }
}
