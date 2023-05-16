using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests.Repositories
{
    public class MockIPostRepository : Mock<IPostRepository>
    {
        public MockIPostRepository MockGetPostById(Post result)
        {
            Setup(x => x.GetPostById(It.IsAny<int>())).ReturnsAsync(result);
            return this;
        }
        public MockIPostRepository MockGetPostByIdInvalid()
        {
            Setup(x => x.GetPostById(It.IsAny<int>())).Throws(new Exception());
            return this;
        }

        public static Mock<IPostRepository> GetPostsMock()
        {
            var mock = new Mock<IPostRepository>();
            var posts = GenerateData(5);
            mock.Setup(m => m.GetPostsAsync().Result).Returns(posts);
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
}