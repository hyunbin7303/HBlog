using HBlog.Api.Controllers;
using HBlog.Application.Services;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using HBlog.Domain.Params;
using HBlog.TestUtilities;
using HBlog.UnitTests.Mocks.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HBlog.UnitTests.Controllers
{
    public class UsersControllerTest : TestBase
    {
        private Mock<IUserService> _userService;
        private UsersController _controller;

        [SetUp]
        public void Init()
        {
            _userService = new Mock<IUserService>();
            _controller = new UsersController(_userService.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = UserSetup() };
        }

        [Test]
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
    }
}
