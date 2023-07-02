using AutoMapper;
using KevBlog.Api.Controllers;
using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Helpers;
using KevBlog.UnitTests.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace KevBlog.UnitTests.Controllers
{
    public class UsersControllerTest : TestBase
    {
        private readonly Mock<IUserService> _userService;
        private UsersController _controller;
        public UsersControllerTest()
        {
            _userService = new Mock<IUserService>();
            _controller = new UsersController(_userService.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = UserSetup() };
        }


        [Fact]
        public async Task GetUsers_FindingExistingUser_ResultSuccess()
        {
            string inputUserName = "kevin0";
            IEnumerable<User> userList = MockUserRepository.SampleValidUserData(1);
            User user = userList.First();
            MemberDto memberDto = _mapper.Map<MemberDto>(user);
            //_userRepository.Setup(repo => repo.GetUserByUsernameAsync(inputUserName)).ReturnsAsync(user);
            _userService.Setup(s => s.GetMembersByUsernameAsync(inputUserName)).ReturnsAsync(ServiceResult.Success(memberDto));
            UserParams userParam = new UserParams();
            userParam.CurrentUsername = inputUserName;


            var actionResult = await _controller.GetUsers(userParam);


            Assert.NotNull(actionResult);
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            MemberDto resultMember = Assert.IsType<MemberDto>(okObjectResult.Value);
            Assert.Equal(inputUserName, user.UserName);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetUser_PassExistingUserName_ReturnOk()
        {
            string inputUserName = "kevin0";
            IEnumerable<User> userList = MockUserRepository.SampleValidUserData(1);
            User returnValue = userList.First();
            //_userRepository.Setup(repo => repo.GetUserByUsernameAsync(inputUserName)).Returns(Task.FromResult(returnValue));

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
            IEnumerable<User> userList = MockUserRepository.SampleValidUserData(1);
            User returnValue = userList.First();
            //_userRepository.Setup(repo => repo.GetUserByUsernameAsync(ExistingUser)).Returns(Task.FromResult(returnValue));

            // Act
            ActionResult<MemberDto> actionResult = await _controller.GetUser(inputUserName);

            // Assert
            var notFoundObj = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundObj.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_UpdateUserInfo_SuccessReturnOk()
        {
            IEnumerable<User> userList = MockUserRepository.SampleValidUserData(3);
            //_userRepository.Setup(repo => repo.GetUsersAsync()).Returns(Task.FromResult(userList));
            //var test = await _userRepository.Object.GetUsersAsync();

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
            var user = MockUserRepository.SampleValidUserData(1);

            User ActualUser = null;
            //_userRepository.Setup(_ => _.Update(It.IsAny<User>())).Callback(new InvocationAction(i => ActualUser = (User)i.Arguments[0]));

        }

    }
}
