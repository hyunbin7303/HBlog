using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
namespace HBlog.UnitTests.Services
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
    }
}
