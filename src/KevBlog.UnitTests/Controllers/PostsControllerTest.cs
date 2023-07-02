using KevBlog.Api.Controllers;
using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Constants;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.UnitTests.Mocks.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;

namespace KevBlog.UnitTests.Controllers
{
    public class PostsControllerTest : TestBase
    {
        private readonly PostsController _controller;
        private readonly Mock<IPostRepository> postRepositoryMock = new();
        private readonly Mock<IUserRepository> userRepositoryMock = new();
        private readonly Mock<IPostService> postServiceMock = new();
        public PostsControllerTest()
        {

            userRepositoryMock.Setup(x => x.GetUserByIdAsync(1)).Returns(Task.FromResult(GetUserFake(1)));
            _controller = new PostsController(postServiceMock.Object, postRepositoryMock.Object);
            _controller.ControllerContext = new ControllerContext { HttpContext = UserSetup() };
        }

        [Fact]
        public async Task GivenPassValidPost_GetPostById__ThenOkWithObject()
        {
            var fakePostId = 1;
            PostDisplayDetailsDto detailDto = new PostDisplayDetailsDto { Id = fakePostId, Title = "PostDto", Desc = "Post Desc", Status = "Created", UserName = "hyunbin7303" };
            postServiceMock.Setup(service => service.GetByIdAsync(fakePostId))
                            .ReturnsAsync(ServiceResult.Success(detailDto));

            ActionResult<PostDisplayDetailsDto> post = await _controller.GetPostById(fakePostId);

            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(post.Result);
            PostDisplayDetailsDto postResult = Assert.IsType<PostDisplayDetailsDto>(okObjectResult.Value);
            Assert.Equal(1, postResult.Id);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetPostById_GivenNotExistPostId_ReturnNotFound()
        {
            // Arrange
            var fakePostId = 100;
            string failureMessage = "Post is not exist or status is removed."; 
            postServiceMock.Setup(service => service.GetByIdAsync(fakePostId))
                            .ReturnsAsync(ServiceResult.Fail<PostDisplayDetailsDto>(msg: failureMessage));

            ActionResult<PostDisplayDetailsDto> post = await _controller.GetPostById(fakePostId);

            NotFoundObjectResult result = Assert.IsType<NotFoundObjectResult>(post.Result);
            Assert.Equal(failureMessage, result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GetPosts_ListofPosts_ReturnSuccess()
        {
            IEnumerable<Post> samplePosts = MockPostRepository.GenerateData(5);
            postRepositoryMock.Setup(x => x.GetPostsAsync()).Returns(Task.FromResult(samplePosts));

            ActionResult<IEnumerable<PostDisplayDto>> posts = await _controller.GetPosts();

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(posts.Result);
            IEnumerable<PostDisplayDto> user = Assert.IsAssignableFrom<IEnumerable<PostDisplayDto>>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdatePost_ArgumentNull_ThrowException()
        {
            Task Act() => _controller.Put(null);

            await Assert.ThrowsAsync<ArgumentNullException>(Act);
        }

        [Fact]
        public async Task UpdatePost_PostIdNull_BadRequest()
        {
            PostUpdateDto postUpdate = new()
            {
                Title = "New Title",
                Desc = "New Desc",
            };

            var result = await _controller.Put(postUpdate);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
        }

        [Fact]
        public async Task UpdateExistingPost_ValidPostIdAndJsonBody_ReturnSuccess()
        {

        }

        [Fact]
        public void CreatePost_PassNull_ThrowArgumentNullException()
        {

            Action act = () => _controller.Create(null);

            // Assert.Equal(StatusCodes.Status200OK, result)
            Assert.Throws<ArgumentNullException>(() => act);
        }

        private Post CreateFakePost(int fakePostId, int fakeUserId)
        {
            return new Post()
            {
                Id = fakePostId,
                Title = "Test",
                Desc = "Test Desc",
                LinkForPost = "https://github.com/hyunbin7303/Dotnet.KevBlog",
                UserId = fakeUserId
            };
        }
    }
}
