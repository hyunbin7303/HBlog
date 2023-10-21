using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.UnitTests.Mocks.Repositories;
using Moq;


namespace KevBlog.UnitTests.Services
{
    public class PostServiceTest : TestBase
    {
        private IPostService _postService;
        private readonly MockPostRepository _mockPostRepository = new MockPostRepository();
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IRepository<PostTags>> _postTagsRepositoryMock;
        public PostServiceTest()
        {
            _userRepositoryMock = new();
            _tagRepositoryMock = new();
            _postTagsRepositoryMock = new();
            _postService = new PostService(_mapper, _mockPostRepository.Object, _userRepositoryMock.Object, _tagRepositoryMock.Object, _postTagsRepositoryMock.Object);
        }
        [Fact]
        public async Task GetPosts_ExistingInRepo_ReturnSuccess()
        {
            int howMany = 5;
            var testObject = MockPostRepository.GenerateData(howMany);
            _mockPostRepository.Setup(x => x.GetPostsAsync()).ReturnsAsync(testObject);

            var posts = await _postService.GetPosts();

            Assert.NotNull(posts);
            Assert.Equal(howMany, posts.Count());
        }

        [Fact]
        public async Task GivenExistingPostId_WhenGetPostById_ReturnPostSuccessfully()
        {
            int postId = 1;
            _mockPostRepository.MockGetPostById(new Post { Id = postId, Title = "new Post mocking" });

            var postDetails = await _postService.GetByIdAsync(1);

            Assert.True(postDetails.IsSuccess);
            Assert.IsType<PostDisplayDetailsDto>(postDetails.Value);
            _mockPostRepository.Verify(x => x.GetById(postId));
        }

        [Fact]
        public async Task GivenNotExistingPostId_WhenGetPostById_ThenReturnNotFound()
        {
            var postDetails = await _postService.GetByIdAsync(2);

            Assert.False(postDetails.IsSuccess);
            Assert.Equal("Post is not exist or status is removed.", postDetails.Message);
        }

        [Fact]
        public async Task GivenValidPostAndExistingUser_WhenCreatePost_ThenIsSuccessTrue()
        {
            string userName = "kevin0";
            PostCreateDto postCreateDto = new PostCreateDto()
            {
                Title = "New Post Create",
                Desc = "New Post Desc",
                LinkForPost = "https://github.com/hyunbin7303",
                Type = "Programming"
            };
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(userName)).ReturnsAsync(new User { Id = 1, Email = "hyunbin7303@gmail.com", UserName = userName });

            // Action 
            var result = await _postService.CreatePost(userName, postCreateDto);

            Assert.True(result.IsSuccess);
            Assert.True(result.Message?.Contains("Post Id"));
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync(userName), Times.Once);
            _mockPostRepository.Verify(x => x.Add(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task GivenValidPostAndExistingUser_WhenCreatePost_ThenIsSuccessFalseAndErrorMessage()
        {
            string userName = "kevin0";
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(userName)).ReturnsAsync(new User { Id = 1, Email = "hyunbin7303@gmail.com", });

            var result = await _postService.CreatePost(userName, new PostCreateDto { Title = null });

            Assert.False(result.IsSuccess);
            Assert.Equal("Title cannot be empty.", result.Message);
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync("kevin0"), Times.Never);
            _mockPostRepository.Verify(x => x.Add(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePost_ValidPostUpdate_ResultReturnTrue()
        {
            var testObject = MockPostRepository.GenerateData(5)[0];
            int postId = testObject.Id;
            PostUpdateDto postUpdateDto = new PostUpdateDto()
            {
                Id = postId,
                Title = "Post Update DTO Update",
                Desc = "Post Update new Desc",
                Content = "Content new info",
                LinkForPost = "",
                Type = "Programming"
            };
            _mockPostRepository.Setup(x => x.GetById(postId)).ReturnsAsync(testObject);

            var result = await _postService.UpdatePost(postUpdateDto); 

            Assert.Equal(true, result.IsSuccess);
            _mockPostRepository.Verify(x => x.GetById(postId), Times.Once);
            _mockPostRepository.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePost_NotExistingPost_ResultReturnFalse()
        {
            var testObject = MockPostRepository.GenerateData(5)[0];
            int postId = testObject.Id;
            int notExistingId = 2;
            PostUpdateDto postUpdateDto = new PostUpdateDto()
            {
                Id = notExistingId,
                Title = "Post Update DTO Update",
                Desc = "Post Update new Desc",
                Content = "Content new info",
                LinkForPost = "",
                Type = "Programming"
            };
            _mockPostRepository.Setup(x => x.GetById(postId)).ReturnsAsync(testObject);

            var result = await _postService.UpdatePost(postUpdateDto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Post does not exist.", result.Message);
            _mockPostRepository.Verify(x => x.GetById(notExistingId), Times.Once);
            _mockPostRepository.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Never);
        }
        [Fact]
        public async Task GivenExistingItem_WhenDeletePost_ThenReturnTrue()
        {
            int postId = 1;
            _mockPostRepository.Setup(x => x.GetById(postId)).ReturnsAsync(new Post
            {
                Id = postId,
                Title = "Post New Title",
            });

            var result = await _postService.DeletePost(1);

            Assert.True(result.IsSuccess);
            Assert.Equal($"Removed Post Id: {postId}", result.Message);
            _mockPostRepository.VerifyGetPostById(Times.Once());
        }

        [Fact]
        public async Task GivenNotExistingPostId_WhenDeletePost_ThenReturnFalse()
        {
            int postId = 1;
            int invalidPostId = 2;
            _mockPostRepository.Setup(x => x.GetById(postId)).ReturnsAsync(new Post
            {
                Id = postId,
                Title = "Post New Title",
            });

            var result = await _postService.DeletePost(invalidPostId);

            Assert.False(result.IsSuccess);
            Assert.Equal($"NotFound", result.Message);
            _mockPostRepository.VerifyGetPostById(Times.Once());
        }

        [Fact]
        public async Task Given_WhenAddTagForPost_Then()
        {
            int postId = 1;
            int tagId = 1;

            await _postService.AddTagForPost(postId, tagId);
        }
    }
}
