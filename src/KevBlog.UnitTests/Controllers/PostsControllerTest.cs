using KevBlog.Api.Controllers;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Constants;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.UnitTests.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KevBlog.UnitTests.Controllers
{
    public class PostsControllerTest : TestBase
    {
        private readonly PostsController controller;
        private readonly Mock<IPostRepository> postRepositoryMock;
        private readonly Mock<IUserRepository> userRepositoryMock;
        private readonly Mock<ITagRepository> tagRepositoryMock;
        public PostsControllerTest()
        {

            postRepositoryMock = new Mock<IPostRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByIdAsync(1)).Returns(Task.FromResult(GetUserFake(1)));
            tagRepositoryMock = new Mock<ITagRepository>();

            controller = new PostsController(postRepositoryMock.Object, userRepositoryMock.Object, tagRepositoryMock.Object, _mapper);
            controller.ControllerContext = new ControllerContext { HttpContext = UserSetup() };
        }

        [Fact]
        public async Task GetPostById_PassValidPost_ReturnSuccess()
        {
            // Arrange
            var fakeUserId = 1;
            var fakePostId = 1;
            var fakeUserPost = CreateFakePost(fakePostId, fakeUserId);
            postRepositoryMock.Setup(x => x.GetPostById(It.IsAny<int>())).Returns(Task.FromResult(fakeUserPost));

            // Act
            ActionResult<PostDisplayDetailsDto> post = await controller.GetPostById(fakeUserPost.Id);

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(post.Result);
            PostDisplayDetailsDto postResult = Assert.IsType<PostDisplayDetailsDto>(okObjectResult.Value);
            Assert.Equal(1, postResult.Id);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetPostById_NotExistPostId_ReturnNotFound()
        {
            // Arrange
            var fakePostId = 100;
            var fakeUserId = 1;
            var fakeUserPost = CreateFakePost(1, fakeUserId);
            postRepositoryMock.Setup(repo => repo.GetPostById(1)).Returns(Task.FromResult(fakeUserPost));

            // Act
            ActionResult<PostDisplayDetailsDto> post = await controller.GetPostById(fakePostId);

            // Assert
            NotFoundResult okObjectResult = Assert.IsType<NotFoundResult>(post.Result);
            Assert.Equal(StatusCodes.Status404NotFound, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetPostById_PostStatusRemoved_ReturnNotFound()
        {
            // Arrange
            var fakePostId = 1;
            var fakeUserId = 1;
            var fakeUserPost = CreateFakePost(fakePostId, fakeUserId);
            fakeUserPost.Status = PostStatus.Removed;
            postRepositoryMock.Setup(repo => repo.GetPostById(1)).Returns(Task.FromResult(fakeUserPost));

            // Act
            ActionResult<PostDisplayDetailsDto> post = await controller.GetPostById(fakePostId);

            // Assert
            NotFoundResult okObjectResult = Assert.IsType<NotFoundResult>(post.Result);
            Assert.Equal(StatusCodes.Status404NotFound, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task CreatePostTag_CreateTagInExistingPost_Success()
        {
            string tag = "";

        }


        [Fact]
        public async Task GetPosts_ListofPosts_ReturnSuccess()
        {
            IEnumerable<Post> samplePosts = MockIPostRepository.GenerateData(5);
            postRepositoryMock.Setup(x => x.GetPostsAsync()).Returns(Task.FromResult(samplePosts));
            var controller = new PostsController(postRepositoryMock.Object, userRepositoryMock.Object, tagRepositoryMock.Object, _mapper);

            ActionResult<IEnumerable<PostDisplayDto>> posts = await controller.GetPosts();

            // Assert
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(posts.Result);
            IEnumerable<PostDisplayDto> user = Assert.IsAssignableFrom<IEnumerable<PostDisplayDto>>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
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
