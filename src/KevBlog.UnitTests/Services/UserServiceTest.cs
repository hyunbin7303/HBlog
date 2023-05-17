using KevBlog.Application.Services;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var result = await _userService.GetMembersAsync(userParams);

            Assert.NotNull(result);
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync("kevin0"), Times.Once);
        }
        [Fact]
        public void Test2()
        {
            // Arrange

            //Act

            //Assert
        }
    }
}
