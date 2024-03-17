using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using KevBlog.UnitTests.Mocks.Repositories;
using Moq;
using NUnit.Framework;

namespace KevBlog.UnitTests.Services
{
    public class UserServiceTest : TestBase
    {
        private IUserService _userService;
        private readonly MockUserRepository _userRepositoryMock = new(); // Just use this?
        public UserServiceTest()
        {
            _userService = new UserService(_mapper, _userRepositoryMock.Object);
        }

        [Test]
        public async Task GetMembersAsync_ExistingUser_ReturnPageList()
        {
            string username = "kevin0";
            UserParams userParams = new UserParams { Gender = "male", PageNumber=0, PageSize = 5 };
            userParams.CurrentUsername = username;

            var result = await _userService.GetMembersAsync(userParams);

            Assert.That(result, Is.Not.Null);
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync(username), Times.Once);
        }

        [Test]
        public async Task GetMembersByUsernameAsync_ExistingUser_ReturnMemberDto()
        {
            string username = "kevin0";
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(username)).ReturnsAsync(new User { Id = 1, UserName = username });

            var result = await _userService.GetMembersByUsernameAsync(username);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.UserName, Is.EqualTo(username));
            Assert.That(result.Value.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetMembersByUsernameAsync_NotExistingUser_ResultFailure()
        {
            string username = "NonExisting";
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync("kevin0")).ReturnsAsync(new User { Id = 1, UserName = "kevin0" });

            var result = await _userService.GetMembersByUsernameAsync(username);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Failed to get user"));
        }
    }
}
