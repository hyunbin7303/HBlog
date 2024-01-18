using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;


namespace KevBlog.UnitTests.Services
{
    public class MessageServiceTest : TestBase
    {
        private IMessageService _msgService;
        private readonly Mock<IMessageRepository> _msgRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IGroupRepository> _groupRepositoryMock;
        public MessageServiceTest()
        {
            _msgRepositoryMock = new();
            _userRepositoryMock = new();
            _groupRepositoryMock = new();
            _msgService = new MessageService(_mapper, _msgRepositoryMock.Object, _userRepositoryMock.Object, _groupRepositoryMock.Object);
        }

        [Fact]
        public async Task GivenSameNameForUserAndRecipient_WhenCreatemessageCalled_ThenResultFail()
        {
            string username = "test";
            
            var result = await _msgService.CreateMessage(username, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = username });

            Assert.False(result.IsSuccess);
            Assert.Equal("You cannot send messages to yourself.", result.Message);
            _msgRepositoryMock.Verify(o => o.AddMessage(It.IsAny<Message>()), Times.Never);
        }

        [Fact]
        public async Task GivenUserExist_AndNoRecipient_WhenCreateMessage_ThenResultFail()
        {
            string validUser = "validUser01";
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(validUser)).ReturnsAsync(new User { Id =1, UserName = validUser, });
          
            var result = await _msgService.CreateMessage(validUser, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = "Recipient01" });

            Assert.False(result.IsSuccess);
            Assert.Equal("Recipient not found", result.Message);
        }

        [Fact]
        public async Task GivenSaveAllFailure_WhenCreateMessage_ThenResultFail()
        {
            string validUser = "validUser01";
            string recipentUser = "validRecipentUser01";
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(validUser)).ReturnsAsync(new User { Id = 1, UserName = validUser, Email = "validUser01@gmail.com" });
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(recipentUser)).ReturnsAsync(new User { Id = 1, UserName = recipentUser, Email = "Recipient@gmail.com" });
            _msgRepositoryMock.Setup(o => o.SaveAllAsync()).ReturnsAsync(false);    

            var result = await _msgService.CreateMessage(validUser, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = recipentUser });

            Assert.False(result.IsSuccess);
            Assert.Equal("Error in Create Message.", result.Message);
        }

        [Fact]
        public async Task GivenSaveAllSuccess_WhenCreateMessage_ThenResultSuccess()
        {
            string validUser = "validUser01";
            string recipentUser = "validRecipentUser01";
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(validUser)).ReturnsAsync(new User { Id = 1, UserName = validUser, Email = "validUser01@gmail.com" });
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(recipentUser)).ReturnsAsync(new User { Id = 1, UserName = recipentUser, Email = "Recipient@gmail.com" });
            _msgRepositoryMock.Setup(o => o.SaveAllAsync()).ReturnsAsync(true);    

            var result = await _msgService.CreateMessage(validUser, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = recipentUser });

            Assert.True(result.IsSuccess);
            Assert.Equal(result.Value.SenderUsername, validUser);
            Assert.Equal(result.Value.RecipientUsername, recipentUser);
            _msgRepositoryMock.Verify(o => o.AddMessage(It.IsAny<Message>()), Times.Once);
            _msgRepositoryMock.Verify(o => o.SaveAllAsync(), Times.Once);
        }
    }
}
