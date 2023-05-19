using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests.Services
{
    public class UserServiceTest : ServiceTest
    {
        private IUserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        public UserServiceTest()
        {
            _userService = new UserService(_mapper, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetMembersAsync_ExistingUser_ReturnPageList()
        {
            string username = "kevin0";
            UserParams userParams = new UserParams();
            userParams.CurrentUsername = username;

            var result = await _userService.GetMembersAsync(userParams);

            Assert.NotNull(result);
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync(username), Times.Once);
        }

        [Fact]
        public async Task GetMembersByUsernameAsync_ExistingUser_ReturnMemberDto()
        {
            string username = "kevin0";
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(username)).ReturnsAsync(new User { Id = 1, UserName = username });

            var result = await _userService.GetMembersByUsernameAsync(username);

            Assert.True(result.IsSuccess);
            Assert.Equal(username, result.Value.UserName);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async Task GetMembersByUsernameAsync_NotExistingUser_ResultFailure()
        {
            string username = "NonExisting";
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync("kevin0")).ReturnsAsync(new User { Id = 1, UserName = "kevin0" });

            var result = await _userService.GetMembersByUsernameAsync(username);

            Assert.False(result.IsSuccess);
            Assert.Equal("Failed to get user", result.Message);
        }
    }
}
