using KevBlog.Application.Services;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;
using NUnit.Framework;

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

        [Test]
        public async Task GivenSameNameForUserAndRecipient_WhenCreatemessageCalled_ThenResultFail()
        {
            string username = "test";
            
            var result = await _msgService.CreateMessage(username, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = username });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("You cannot send messages to yourself."));
            _msgRepositoryMock.Verify(o => o.AddMessage(It.IsAny<Message>()), Times.Never);
        }

        [Test]
        public async Task GivenUserExist_AndNoRecipient_WhenCreateMessage_ThenResultFail()
        {
            string validUser = "validUser01";
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(validUser)).ReturnsAsync(new User { Id =1, UserName = validUser, });
          
            var result = await _msgService.CreateMessage(validUser, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = "Recipient01" });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Recipient not found"));
        }

        [Test]
        public async Task GivenSaveAllFailure_WhenCreateMessage_ThenResultFail()
        {
            string validUser = "validUser01";
            string recipentUser = "validRecipentUser01";
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(validUser)).ReturnsAsync(new User { Id = 1, UserName = validUser, Email = "validUser01@gmail.com" });
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(recipentUser)).ReturnsAsync(new User { Id = 1, UserName = recipentUser, Email = "Recipient@gmail.com" });
            _msgRepositoryMock.Setup(o => o.SaveAllAsync()).ReturnsAsync(false);    

            var result = await _msgService.CreateMessage(validUser, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = recipentUser });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Error in Create Message."));
        }

        [Test]
        public async Task GivenSaveAllSuccess_WhenCreateMessage_ThenResultSuccess()
        {
            string validUser = "validUser01";
            string recipentUser = "validRecipentUser01";
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(validUser)).ReturnsAsync(new User { Id = 1, UserName = validUser, Email = "validUser01@gmail.com" });
            _userRepositoryMock.Setup(o => o.GetUserByUsernameAsync(recipentUser)).ReturnsAsync(new User { Id = 1, UserName = recipentUser, Email = "Recipient@gmail.com" });
            _msgRepositoryMock.Setup(o => o.SaveAllAsync()).ReturnsAsync(true);    

            var result = await _msgService.CreateMessage(validUser, new MessageCreateDto { Content = "YOYOYO", RecipientUsername = recipentUser });

            Assert.That(result.IsSuccess,Is.True);
            Assert.That(result.Value.SenderUsername, Is.EqualTo(validUser));
            Assert.That(result.Value.RecipientUsername, Is.EqualTo(validUser));
            _msgRepositoryMock.Verify(o => o.AddMessage(It.IsAny<Message>()), Times.Once);
            _msgRepositoryMock.Verify(o => o.SaveAllAsync(), Times.Once);
        }
    }
}
