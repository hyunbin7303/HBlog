using AutoFixture;
using AutoMapper;
using KevBlog.Api.Controllers;
using KevBlog.Application.Automapper;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevBlog.UnitTests
{
    public class UsersControllerTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IPostRepository> _postRepository;
        private Fixture _fixture;
        private UsersController _controller;
        private readonly IMapper _mapper;
        public UsersControllerTests()
        {
            _fixture= new Fixture();
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
        public async Task Get_Post_ReturnOk()
        {
            var postList = _fixture.CreateMany<Post>(3).ToList();

            _postRepository.Setup(repo => repo.GetPostsAsync().Result).Returns(postList);
        }
        [Fact]
        public async Task GetUsersAsync_RetrieveData_ReturnOk()
        {
            IEnumerable<User> userList = _fixture.CreateMany<User>(5).ToList();

            _userRepository.Setup(repo => repo.GetUsersAsync().Result).Returns(userList);

            _controller = new UsersController(_userRepository.Object, _mapper);

            var result = await _controller.GetUser("hyunbin7303");
            //var obj1 = result as ObjectResult;
            //Assert.Equal(200, result.)
        }

        //[Fact]
        //public async Task GetUserAsync_
    }
}
