using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using KevBlog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace KevBlog.UnitTests.Repositories
{
    public class PostRepositoryTest
    {
        private IPostRepository _repository;    
        private readonly Mock<DataContext> dataContextMock = new();
        public PostRepositoryTest()
        {
            _repository = new PostRepository(dataContextMock.Object);
        }


        [Fact]
        public async Task GetPostsAsync_Retrieve_Success()
        {
            int postTotal = 5;
            var mock = MockIPostRepository.GenerateData(postTotal).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var posts = await _repository.GetPostsAsync();

            Assert.NotNull(posts);
            Assert.Equal(postTotal, posts.Count());
        }

        [Fact]
        public async Task GetPostById_FindExistingOne_Success()
        {
            var mock = MockIPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var post = await _repository.GetPostById(1);

            Assert.NotNull(post);
            Assert.Equal(1, post.Id);
        }

        [Fact]
        public async Task GetPostById_NotExistingId_ReturnNull()
        {
            var mock = MockIPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var post = await _repository.GetPostById(100);

            Assert.Null(post);
        }

        [Fact]
        public async Task GetPostsByUserName_ExistingUser_ReturnPosts()
        {
            var mock = MockIPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var posts = await _repository.GetPostsByUserName("test");

            Assert.NotNull(posts);
            Assert.Equal(1, posts.First().UserId);
        }

        [Fact]
        public async Task GetPostsByUserName_NotExistingUser_ReturnNull()
        {
            var mock = MockIPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var posts = await _repository.GetPostsByUserName("NotExisting");

            Assert.Null(posts);
        }


        [Fact]
        public async Task VerifyingMockWorking()
        {
            // Arrange
            var testObject = MockIPostRepository.GenerateData(5)[0];

            var context = new Mock<DataContext>();
            //var dbSetMock = new Mock<DbSet<Post>>();
            var mock = MockIPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            mock.Setup(x => x.Find(It.IsAny<int>())).Returns(testObject);
            context.Setup(x => x.Set<Post>()).Returns(mock.Object);

            // Act
            var repository = new PostRepository(context.Object);
            var result = await repository.GetPostsAsync();

            // Assert
            context.Verify(x => x.Set<Post>());
            mock.Verify(x => x.Find(It.IsAny<int>()));
        }
    }
}