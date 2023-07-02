using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests.Repositories
{
    public class MockPostRepository : Mock<IPostRepository>
    {
        public MockPostRepository MockGetPostById(Post result)
        {
            Setup(x => x.GetPostById(It.IsAny<int>())).ReturnsAsync(result);
            return this;
        }
        public MockPostRepository MockGetPostByIdInvalid()
        {
            Setup(x => x.GetPostById(It.IsAny<int>())).Throws(new Exception());
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
            Verify(x => x.GetPostById(It.IsAny<int>()), times);
            return this;
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