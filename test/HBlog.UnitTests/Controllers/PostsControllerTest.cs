using HBlog.Api.Controllers;
using HBlog.Application.Services;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common.Params;
using HBlog.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HBlog.UnitTests.Controllers
{
    public class PostsControllerTest : TestBase
    {
        private PostsController _controller;
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IPostService> postServiceMock = new();

        [SetUp]
        public void Init()
        {
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(1)).Returns(Task.FromResult(GetUserFake(1)));
            _controller = new PostsController(postServiceMock.Object, userRepositoryMock.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = UserSetup() };
        }

        [Test]
        public async Task GivenPassValidPost_GetPostById__ThenOkWithObject()
        {
            var fakePostId = 1;
            PostDisplayDetailsDto detailDto = new PostDisplayDetailsDto { Id = fakePostId, Title = "PostDto", Desc = "Post Desc", Status = "Created", UserName = "hyunbin7303" };
            postServiceMock.Setup(service => service.GetByIdAsync(fakePostId))
                            .ReturnsAsync(ServiceResult.Success(detailDto));

            ActionResult<PostDisplayDetailsDto> post = await _controller.GetPostById(fakePostId);

            Assert.That(post.Result, Is.TypeOf<OkObjectResult>());
            //Assert.That(postResult.Id, Is.EqualTo(1));
            //Assert.That(okObjectResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        public async Task GetPostById_GivenNotExistPostId_ReturnNotFound()
        {
            // Arrange
            var fakePostId = 100;
            string failureMessage = "Post is not exist or status is removed."; 
            postServiceMock.Setup(service => service.GetByIdAsync(fakePostId))
                            .ReturnsAsync(ServiceResult.Fail<PostDisplayDetailsDto>(msg: failureMessage));

            ActionResult<PostDisplayDetailsDto> post = await _controller.GetPostById(fakePostId);

            Assert.That(post.Result, Is.TypeOf<NotFoundObjectResult>());
            //Assert.That(result.Value, Is.EqualTo(failureMessage));
            //Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public async Task GetPosts_ListofPosts_ReturnSuccess()
        {
            ActionResult<IEnumerable<PostDisplayDto>> posts = await _controller.GetPosts(new PostParams());

            Assert.That(posts.Result, Is.TypeOf<OkObjectResult>());
        }

        //[Test]
        //public async Task UpdatePost_ArgumentNull_ThrowException()
        //{
        //    Task Act() => _controller.Put(null);

        //    await Assert.ThrowsAsync<ArgumentNullException>(Act);
        //}

        //[Test]
        //public async Task UpdatePost_PostIdNull_BadRequest()
        //{
        //    PostUpdateDto postUpdate = new()
        //    {
        //        Title = "New Title",
        //        Desc = "New Desc",
        //    };

        //    var result = await _controller.Put(postUpdate);

        //    var badRequest = Assert.IsType<BadRequestObjectResult>(result);

        //    Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        //}
    }
}
