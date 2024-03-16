using KevBlog.Application;
using KevBlog.Application.Services;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Common.Params;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.UnitTests.Mocks.Repositories;
using Moq;
using NUnit.Framework;


namespace KevBlog.UnitTests.Services
{
    public class PostServiceTest : TestBase
    {
        private IPostService _postService;
        private readonly MockPostRepository _mockPostRepository = new MockPostRepository();
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryMock;
        private readonly Mock<IRepository<PostTags>> _postTagsRepositoryMock;
        public PostServiceTest()
        {
            _userRepositoryMock = new();
            _tagRepositoryMock = new();
            _postTagsRepositoryMock = new();
            _categoryMock = new();
            _postService = new PostService(_mapper, _mockPostRepository.Object, _userRepositoryMock.Object, _tagRepositoryMock.Object, 
                _postTagsRepositoryMock.Object, _categoryMock.Object);
        }
        [Test]
        public async Task GetPosts_ExistingInRepo_ReturnSuccess()
        {
            int howMany = 5;
            var testObject = MockPostRepository.GenerateData(howMany);
            _mockPostRepository.Setup(x => x.GetPostsAsync()).ReturnsAsync(testObject);

            var posts = await _postService.GetPosts(new QueryParams());

            Assert.That(posts.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task GivenExistingPostId_WhenGetPostById_ReturnPostSuccessfully()
        {
            int postId = 1;
            _mockPostRepository.MockGetPostById(new Post { Id = postId, Title = "new Post mocking" });

            var postDetails = await _postService.GetByIdAsync(1);

            Assert.That(postDetails.IsSuccess, Is.True);
            //Assert.That(postDetails.Value, Is.InstanceOf(Type.));
            _mockPostRepository.Verify(x => x.GetById(postId));
        }

        [Test]
        public async Task GivenNotExistingPostId_WhenGetPostById_ThenReturnNotFound()
        {
            var postDetails = await _postService.GetByIdAsync(2);

            Assert.That(postDetails.IsSuccess, Is.False);
            Assert.That(postDetails.Message, Is.EqualTo("Post is not exist or status is removed."));
        }

        [Test]
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

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message?.Contains("Post Id"), Is.True);
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync(userName), Times.Once);
            _mockPostRepository.Verify(x => x.Add(It.IsAny<Post>()), Times.Once);
        }

        [Test]
        public async Task GivenValidPostAndExistingUser_WhenCreatePost_ThenIsSuccessFalseAndErrorMessage()
        {
            string userName = "kevin0";
            _userRepositoryMock.Setup(x => x.GetUserByUsernameAsync(userName)).ReturnsAsync(new User { Id = 1, Email = "hyunbin7303@gmail.com", });

            var result = await _postService.CreatePost(userName, new PostCreateDto { Title = null });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Title cannot be empty."));
            _userRepositoryMock.Verify(x => x.GetUserByUsernameAsync("kevin0"), Times.Never);
            _mockPostRepository.Verify(x => x.Add(It.IsAny<Post>()), Times.Never);
        }

        [Test]
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

            Assert.That(result.IsSuccess, Is.True);
            _mockPostRepository.Verify(x => x.GetById(postId), Times.Once);
            _mockPostRepository.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Once);
        }

        [Test]
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

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Post does not exist."));
            _mockPostRepository.Verify(x => x.GetById(notExistingId), Times.Once);
            _mockPostRepository.Verify(x => x.UpdateAsync(It.IsAny<Post>()), Times.Never);
        }

        [Test]
        public async Task GivenExistingItem_WhenDeletePost_ThenReturnTrue()
        {
            int postId = 1;
            _mockPostRepository.Setup(x => x.GetById(postId)).ReturnsAsync(new Post
            {
                Id = postId,
                Title = "Post New Title",
            });

            var result = await _postService.DeletePost(1);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo($"Removed Post Id: {postId}"));
            _mockPostRepository.VerifyGetPostById(Times.Once());
        }

        [Test]
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

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo($"NotFound"));
            _mockPostRepository.VerifyGetPostById(Times.Once());
        }

        [Test]
        public async Task Given_WhenAddTagForPost_Then()
        {
            int postId = 1;
            int tagId = 1;

            var result = await _postService.AddTagForPost(postId, tagId);
        }
    }
}
