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
        private Mock<IUserRepository> _userRepositoryMock = new();
        public UserServiceTest()
        {
            _userService = new UserService(_mapper, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetMembersAsync_ExistingUser_ReturnPageList()
        {
            string username = "kevin0";
            UserParams userParams = new UserParams();

            var member = await _userService.GetMembersAsync(userParams);

            Assert.NotNull(member);

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
