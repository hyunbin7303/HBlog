using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace KevBlog.UnitTests
{
    internal class MockIPostRepository
    {
        public static Mock<IPostRepository> GetPostsMock()
        {
            var mock = new Mock<IPostRepository>();
            var posts = new List<Post>()
            {
                new Post {Id = 1, UserId = 1, Created = DateTime.Now, Desc = "Desc1", Content = "Content1", LastUpdated = DateTime.Now, Status = "Pending", LinkForPost = "https://github.com/hyunbin7303"},
                new Post {Id = 2, UserId = 1, Created = DateTime.Now, Desc = "Desc2", Content = "Content2", LastUpdated = DateTime.Now, Status = "Pending", LinkForPost = "https://github.com/hyunbin7303"},
                new Post {Id = 3, UserId = 1, Created = DateTime.Now, Desc = "Desc3", Content = "Content3", LastUpdated = DateTime.Now, Status = "Pending", LinkForPost = "https://github.com/hyunbin7303"},
            };
            return mock;
        }

    }
    public class PostRepositoryTest
    {
        private readonly Mock<IPostRepository> _mockPostRepository;


        public PostRepositoryTest()
        {
            var check = MockIPostRepository.GetPostsMock();
        }

        [Fact]
        public void GetUserInfo_()
        {
            var check = MockIPostRepository.GetPostsMock();

        }
        [Fact]
        public void Update_ExistingPost_Success()
        {
            var check = MockIPostRepository.GetPostsMock();

            //_mockPostRepository.
            Assert.NotNull(check);
            // TODO
        }
        [Fact]
        public void Update_NotExistingPost_ReturnFailure()
        {
            // TODO
        }

        [Fact]
        public async Task Insert_Post_Success()
        {
            var mockdbContext = new Mock<DataContext>();
            var mockSet = new Mock<DbSet<Post>>();
            //TODO 
        }
    }
}