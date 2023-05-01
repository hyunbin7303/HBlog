using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using KevBlog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KevBlog.UnitTests
{
    internal class MockIPostRepository
    {
        // Should I create sample user info.

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
        private readonly PostRepository repository;
        public PostRepositoryTest()
        {
            var dataList = MockIPostRepository.GenerateData(5);
            var data = dataList.AsQueryable();
            var mockSet = new Mock<DbSet<Post>>();
            mockSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockdbContext = new Mock<DataContext>();
            mockdbContext.Setup(x => x.Set<Post>()).Returns(mockSet.Object);
            mockdbContext.Setup(x => x.Posts).Returns(mockSet.Object);
            repository = new PostRepository(mockdbContext.Object);
        }

        [Fact]
        public void Update_NotExistingPost_ReturnFailure()
        {
            // TODO
        }

        [Fact] 
        public async Task GetPost_Success()
        {
            var posts = await repository.GetPostsAsync();

            Assert.NotNull(posts);
            var firstPost = posts.FirstOrDefault();
            Assert.Equal(1, firstPost.Id);
        }
        [Fact]
        public async Task GetPostById_Success()
        {
            Post post =  await repository.GetPostById(1);
            Assert.NotNull(post);
            Assert.Equal(1, post.Id);
        }
    }
}