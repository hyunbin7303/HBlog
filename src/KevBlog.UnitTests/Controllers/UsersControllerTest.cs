using AutoMapper;
using KevBlog.Api.Controllers;
using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace KevBlog.UnitTests.Controllers
{
    internal class MockIUserRepository
    {
        public static Mock<IUserRepository> GetMock()
        {
            var mock = new Mock<IUserRepository>();

            IEnumerable<User> userList = SampleValidUserData(3);
            mock.Setup(m => m.GetUsersAsync().Result).Returns(() => userList);
            return mock;
        }
        public static IEnumerable<User> SampleValidUserData(int howMany)
        {
            List<User> users = new();
            for (int i = 0; i < howMany; i++)
            {
                User user = new()
                {
                    Id = i,
                    UserName = "kevin" + i,
                    KnownAs = "knownas" + i,
                    Gender = "Male",
                    City = "Kitchener",
                    DateOfBirth = new DateTime(1993, 7, 3),
                    Email = "hyunbin7303@gmail.com",
                };
                users.Add(user);
            }
            return users;
        }
    }
    public class UsersControllerTest : TestBase
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUserService> _userService;
        private UsersController _controller;
        public UsersControllerTest()
        {
            _userRepository = new Mock<IUserRepository>();
            _userService = new Mock<IUserService>();
            _controller = new UsersController(_userService.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = UserSetup() };
        }


        [Fact]
        public async Task GetUsersAsync_NotNull()
        {
            var mock = MockIUserRepository.GetMock();

            var userList = await mock.Object.GetUsersAsync();

            Assert.NotNull(userList);
        }

        [Fact]
        public async Task GetUsers_FindingExistingUser()
        {
            //var mock = MockIUserRepository.GetMock();
            string inputUserName = "kevin0";
            IEnumerable<User> userList = MockIUserRepository.SampleValidUserData(1);
            User returnValue = userList.First();
            _userRepository.Setup(repo => repo.GetUserByUsernameAsync(inputUserName)).Returns(Task.FromResult(returnValue));
            UserParams userParam = new UserParams();
            userParam.CurrentUsername = inputUserName;

            var result = await _controller.GetUsers(userParam);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUser_PassExistingUserName_ReturnOk()
        {
            string inputUserName = "kevin0";
            IEnumerable<User> userList = MockIUserRepository.SampleValidUserData(1);
            User returnValue = userList.First();
            _userRepository.Setup(repo => repo.GetUserByUsernameAsync(inputUserName)).Returns(Task.FromResult(returnValue));

            // Act
            ActionResult<MemberDto> actionResult = await _controller.GetUser(inputUserName);

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            MemberDto user = Assert.IsType<MemberDto>(okObjectResult.Value);
            Assert.Equal(inputUserName, user.UserName);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetUser_PassNotExistingUser_NotFound()
        {
            string inputUserName = "IdontExist";
            string ExistingUser = "kevin0";
            IEnumerable<User> userList = MockIUserRepository.SampleValidUserData(1);
            User returnValue = userList.First();
            _userRepository.Setup(repo => repo.GetUserByUsernameAsync(ExistingUser)).Returns(Task.FromResult(returnValue));

            // Act
            ActionResult<MemberDto> actionResult = await _controller.GetUser(inputUserName);

            // Assert
            var notFoundObj = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundObj.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_UpdateUserInfo_SuccessReturnOk()
        {
            IEnumerable<User> userList = MockIUserRepository.SampleValidUserData(3);
            _userRepository.Setup(repo => repo.GetUsersAsync()).Returns(Task.FromResult(userList));
            var test = await _userRepository.Object.GetUsersAsync();

            MemberUpdateDto memberUpdateDto = new MemberUpdateDto();
            memberUpdateDto.Introduction = "Member Update Dto";
            memberUpdateDto.LookingFor = "";
            memberUpdateDto.Interests = "Programming";

            var result = await _controller.Update(memberUpdateDto);
            var obj = result as ObjectResult;

        }

        [Fact]
        public void UpdateTesting()
        {
            var user = MockIUserRepository.SampleValidUserData(1);

            User ActualUser = null;
            _userRepository.Setup(_ => _.Update(It.IsAny<User>())).Callback(new InvocationAction(i => ActualUser = (User)i.Arguments[0]));

        }

    }
}
