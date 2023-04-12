using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests
{
    internal class MockIPostRepository
    {
        public static Mock<IPostRepository> GetMock()
        {
            var mock = new Mock<IPostRepository>();
            var posts = new List<Post>()
            {
                new Post {Id = 1, UserId = 1, Created = DateTime.Now, Desc = "Desc", Content = "Desc", LastUpdated = DateTime.Now, Status = "Pending", LinkForPost = "https://github.com/hyunbin7303"}

            };
            return mock;
        }
    }
    public class PostRepositoryTest
    {
        //private readonly IFixture _fixture;
        private readonly Mock<IPostRepository> _mockPostRepository;

        public PostRepositoryTest()
        {
            var check = MockIPostRepository.GetMock();
        }

        [Fact]
        public void Test1()
        {

        }
    }
}