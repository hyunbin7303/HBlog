using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.TestUtilities;
using Moq;

namespace HBlog.UnitTests.Mocks.Repositories
{
    public class MockPostRepository : Mock<IPostRepository>
    {
        public MockPostRepository MockGetPostById(Post result)
        {
            Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(result);
            return this;
        }
        public MockPostRepository MockGetByIdInvalid()
        {
            Setup(x => x.GetById(It.IsAny<int>())).Throws(new Exception());
            return this;
        }
        public MockPostRepository MockGetPosts(List<Post> results)
        {
            Setup(x => x.GetPostsAsync()).ReturnsAsync(results);
            return this;
        }
        public MockPostRepository VerifyGetPost(Times times)
        {
            Verify(x => x.GetPostsByUserName(It.IsAny<string>()), times);
            return this;
        }
        public MockPostRepository VerifyGetPostById(Times times)
        {
            Verify(x => x.GetById(It.IsAny<int>()), times);
            return this;
        }


        public static List<Post> GenerateData(int count)
        {
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime endDate = new DateTime(2024, 3, 15);

            var posts = new List<Post>();
            for (int i = 1; i <= count; i++)
            {
                var post = new Post 
                { 
                    Id = i, 
                    UserId = 1, 
                    Created = TestHelper.GenerateRandomDateTime(startDate, endDate), 
                    Desc = "Desc" + i, 
                    Content = "Content1", 
                    LastUpdated = DateTime.Now, 
                    Status = "Pending", 
                    CategoryId  = 1,
                    LinkForPost = "https://github.com/hyunbin7303" };
                post.User = new User
                {
                    Id = i,
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