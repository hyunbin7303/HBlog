using System.Reflection;
using HBlog.Domain.Constants;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.TestUtilities;
using Moq;

namespace HBlog.UnitTests.Mocks.Repositories
{
    public class MockPostRepository : Mock<IPostRepository>
    {
        private Post validPost = new()
        {
            Id = 1, 
            Desc = "Post Desc",
            CategoryId = 1,
            Content = "Content1234",
            Created = DateTime.Now,
            LastUpdated = DateTime.Now.AddDays(1),
            Category = new Category
            {
                Id = 1, Title = "Programming", Description = "P", 
            }
        };


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

        public MockPostRepository MockGetPostDetails(int id)
        {
            if (id == 1)
                Setup(x => x.GetPostDetails(1)).ReturnsAsync(validPost);
            else
                Setup(x => x.GetPostDetails(It.IsAny<int>())).Throws(new Exception());

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
            var fields = typeof(PostStatus).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                .ToArray();
            var random = new Random();

            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime endDate = new DateTime(2024, 3, 15);

            var posts = new List<Post>();
            for (int i = 1; i <= count; i++)
            {
                var randomField = fields[random.Next(fields.Length)];
                var post = new Post 
                { 
                    Id = i, 
                    UserId = 1, 
                    Created = TestHelper.GenerateRandomDateTime(startDate, endDate), 
                    Desc = "Desc" + i, 
                    Content = "Content1", 
                    LastUpdated = DateTime.Now, 
                    Status = randomField.GetValue(null).ToString(), 
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