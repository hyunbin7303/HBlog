using AutoMapper;
using KevBlog.Api.Controllers;
using KevBlog.Application.Automapper;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace KevBlog.UnitTests
{
    public class UsersControllerTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IPostRepository> _postRepository;
        private UsersController _controller;
        private readonly IMapper _mapper;
        public UsersControllerTests()
        {
            _userRepository= new Mock<IUserRepository>();
            _postRepository= new Mock<IPostRepository>();
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
        public async Task Get_Post_ReturnOk()
        {
            // var postList = _fixture.CreateMany<Post>(3).ToList();

            // _postRepository.Setup(repo => repo.GetPostsAsync().Result).Returns(postList);
        }

        [Fact]
        public async Task GetUsers_PassValidUserParam_ReturnOk()
        {
            string inputUserName = "kevin0";
            IEnumerable<User> userList = SampleValidUserData(5);
            // _userRepository.Setup(repo => repo.Get)
            // _userRepository.Setup(repo => repo.GetUserLikesQuery)


        }

        [Fact]
        public async Task GetUser_PassExistingUserName_ReturnOk()
        {
            string inputUserName = "kevin0";
            IEnumerable<User> userList = SampleValidUserData(1);
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
        }

        [Fact]
        public async Task GetUser_PassNotExistingUser_NotFound()
        {
            string inputUserName = "IdontExist";
            string ExistingUser = "kevin0";
            IEnumerable<User> userList = SampleValidUserData(1);
            User returnValue = userList.First();
            _userRepository.Setup(repo => repo.GetUserByUsernameAsync(ExistingUser).Result).Returns(returnValue);
            _controller = new UsersController(_userRepository.Object, _mapper);

            // Act
            ActionResult<MemberDto> actionResult = await _controller.GetUser(inputUserName);

            // Assertion
            var notFoundObj = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
            Assert.Equal(404, notFoundObj.StatusCode);
        }



        private static IEnumerable<User> SampleValidUserData(int howMany) {
            List<User> users = new();
            for(int i=0; i<howMany; i++) {
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
}
