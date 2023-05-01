using AutoMapper;
using KevBlog.Api.Controllers;
using KevBlog.Application.Automapper;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace KevBlog.UnitTests
{
    internal class MockIUserRepository
    {
        public static Mock<IUserRepository> GetMock()
        {
            var mock = new Mock<IUserRepository>();

            IEnumerable<User> userList = SampleValidUserData(1);
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
                    KnownAs = "KnownAs" + i,
                    Gender = "Male",
                    City = "Kitchener"
                };
                users.Add(user);
            }
            return users;
        }
    }
    public class UsersControllerTests
    {
        private Mock<IUserRepository> _userRepository;
        private UsersController _controller;
        private readonly IMapper _mapper;
        public UsersControllerTests()
        {
            _userRepository= new Mock<IUserRepository>();
            if(_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutoMapperProfiles());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;   
            }
        }

       

        [Fact]
        public async Task IsMockDataValid()
        {
            var mock = MockIUserRepository.GetMock();
            var userList = await mock.Object.GetUsersAsync();
            Assert.NotNull(userList);
        }

        [Fact]
        public async Task GetAll_FindingExistingUser_ReturnOk()
        {
            var mock = MockIUserRepository.GetMock();
            var controller = new UsersController(mock.Object, _mapper);

            UserParams userParam = new UserParams();
            userParam.CurrentUsername = "kevin0";

            var result = controller.GetUsers(userParam);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetUser_PassExistingUserName_ReturnOk()
        {
            string inputUserName = "kevin0";
            IEnumerable<User> userList = MockIUserRepository.SampleValidUserData(1);
            User returnValue = userList.First();
            _userRepository.Setup(repo => repo.GetUserByUsernameAsync(inputUserName).Result).Returns(returnValue);
            _controller = new UsersController(_userRepository.Object, _mapper);

            // Act
            ActionResult<MemberDto> actionResult = await _controller.GetUser(inputUserName);

            // Assertion
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            MemberDto user = Assert.IsType<MemberDto>(okObjectResult.Value);
            Assert.NotNull(user);
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
            _userRepository.Setup(repo => repo.GetUserByUsernameAsync(ExistingUser).Result).Returns(returnValue);
            _controller = new UsersController(_userRepository.Object, _mapper);

            // Act
            ActionResult<MemberDto> actionResult = await _controller.GetUser(inputUserName);

            // Assertion
            var notFoundObj = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundObj.StatusCode);
        }

        [Fact]
        public void UpdateTesting()
        {
            var user = MockIUserRepository.SampleValidUserData(1);

            User ActualUser = null;
            _userRepository.Setup(_ => _.Update(It.IsAny<User>()))
                .Callback(new InvocationAction(i => ActualUser = (User)i.Arguments[0]));
            
        }

    }
}
