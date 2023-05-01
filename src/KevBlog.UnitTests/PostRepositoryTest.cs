using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using KevBlog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit.Sdk;

namespace KevBlog.UnitTests
{
    internal class MockIPostRepository
    {
        public static Mock<IPostRepository> GetPostsMock()
        {
            var mock = new Mock<IPostRepository>();
            var posts = GenerateData(5);
            mock.Setup(m => m.GetPostsAsync()).Returns(() => posts);
            mock.Setup(m => m.GetPostById(It.IsAny<int>())).Returns((int id) => posts.FirstOrDefault(o => o.Id == id));
            //mock.Setup(m => m.GetPostByUsername(It.IsAny<string>())).Returns((string username) => posts.FirstOrDefault(o => o.User.UserName ==)
            return mock;
        }
        public static List<Post> GenerateData(int count)
        {
            var posts = new List<Post>();
            for (int i = 0; i < count; i++)
            {
                var post = new Post { Id = i + 1, UserId = 1, Created = DateTime.Now, Desc = "Desc" + i, Content = "Content1", LastUpdated = DateTime.Now, Status = "Pending", LinkForPost = "https://github.com/hyunbin7303" };
                post.User = new User
                {
                    Id = 1,
                    UserName = "test",
                    City = "Kitchener",
                    Gender = "Male",
                };
                posts.Add(post);
            }
            return posts;
        }
    }
    public class PostRepositoryTest
    {
        private readonly Mock<IPostRepository> repositoryMock;
        public PostRepositoryTest()
        {
            var dataList = MockIPostRepository.GenerateData(5);
            repositoryMock = new Mock<IPostRepository>();
            repositoryMock.Setup(repo => repo.GetPostsAsync().Result).Returns(dataList);
        }

        [Fact]
        public async Task GetPost_Success()
        {
            var posts = await repositoryMock.Object.GetPostsAsync();

            Assert.NotNull(posts);
        }
        [Fact]
        public void Get_TestClassObjectPassed_ProperMethodCalled()
        {
            // Arrange
            var testObject = new Post();

            var context = new Mock<DataContext>();
            var dbSetMock = new Mock<DbSet<Post>>();

            context.Setup(x => x.Set<Post>()).Returns(dbSetMock.Object);
            dbSetMock.Setup(x => x.Find(It.IsAny<int>())).Returns(testObject);

            // Act
            var repository = new PostRepository(context.Object);
            var result = repository.GetPostsAsync().Result;

            // Assert
            context.Verify(x => x.Set<Post>());
            dbSetMock.Verify(x => x.Find(It.IsAny<int>()));
        }


        [Fact]
        public async Task GetPostById_Success()
        {
            int findId = 1;

            Post post =  await repositoryMock.Object.GetPostById(findId);
            var posts = await repositoryMock.Object.GetPostsAsync();

            Assert.NotNull(post);
            Assert.Equal(1, post.Id);
        }
    }
}