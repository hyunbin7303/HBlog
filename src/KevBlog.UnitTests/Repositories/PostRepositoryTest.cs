using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using KevBlog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using MockQueryable.Moq;
using Moq;
using NuGet.Protocol.Core.Types;
using Xunit.Sdk;

namespace KevBlog.UnitTests.Repositories
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
        private IPostRepository _repository;    
        private Mock<DataContext> dataContextMock;
        public PostRepositoryTest()
        {
            //var dbContextMock = new Mock<DataContext>();
            //var dbSetMock = new Mock<DbSet<Post>>();
            //dbSetMock.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new Post()));
            //dbContextMock.Setup(s => s.Set<Post>()).Returns(dbSetMock.Object);


        }


        [Fact]
        public void GetPostsAsync_Retrieve_Success()
        {
            var dbContextMock = new Mock<DataContext>();
            var dbSetMock = new Mock<DbSet<Post>>();
            //dbSetMock.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new Post()));
            //dbContextMock.Setup(s => s.Set<Post>()).Returns(dbSetMock.Object);


            dataContextMock = new Mock<DataContext>();
            var mock = MockIPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);
            _repository = new PostRepository(dataContextMock.Object);
            var postsMock = MockIPostRepository.GetPostsMock();
        }

        [Fact]
        public async Task GetPost_Success()
        {
            Mock<DataContext> dataContextsMock = new Mock<DataContext>();
            var mock = MockIPostRepository.GenerateData(5).BuildMock();
            //mock.Setup(x => x.GetQueryable()).Returns(MockIPostRepository.GenerateData(5).ToList());
            //mock.Setup(x => x.FindAsync(1)).ReturnsAsync(MockIPostRepository.GenerateData(5).Find(e => e.Id == 1));
            //dataContextMock = new Mock<DataContext>();
            //dataContextsMock.Setup<DbSet<Post>>(x => x.Posts).Returns(mock.Object);
            Mock<PostRepository> postRepoMock = new Mock<PostRepository>();

            var posts = await postRepoMock.Object.GetPostsAsync();

            Assert.NotNull(posts);
        }


        // not working currently
        [Fact]
        public async Task Get_TestClassObjectPassed_ProperMethodCalled()
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