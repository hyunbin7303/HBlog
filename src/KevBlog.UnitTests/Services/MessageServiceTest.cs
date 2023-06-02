using KevBlog.Application.Services;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using KevBlog.UnitTests.Repositories;
using Moq;


namespace KevBlog.UnitTests.Services
{
    public class MessageServiceTest : ServiceTest
    {
        private IMessageService _msgService;
        private readonly Mock<IMessageRepository> _msgRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        public MessageServiceTest()
        {
            _msgService = new MessageService(_mapper, _msgRepositoryMock.Object, _userRepositoryMock.Object);
        }
        [Fact]
        public async Task GetPosts_ExistingInRepo_ReturnSuccess()
        {
            int howMany = 5;
            var testObject = MockIMessageRepository.GenerateData(howMany);
            _msgRepositoryMock.Setup(m => m.GetMessages()).ReturnsAsync(testObject);
            MessageParams messageParams = new MessageParams();

            var posts = await _msgService.GetMessagesForUserPageList(messageParams);

            Assert.NotNull(posts);
        }
    }
}
