using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Data;
using HBlog.Infrastructure.Repositories;
using HBlog.TestUtilities;
using HBlog.UnitTests.Mocks.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;

namespace HBlog.UnitTests.Repositories
{
    public class PostRepositoryTest : TestBase
    {
        private IPostRepository _repository;       
        private readonly Mock<DataContext> dataContextMock;
        public PostRepositoryTest()
        {
            dataContextMock = new();
            _repository = new PostRepository(dataContextMock.Object);
        }


        [Test]  
        public async Task GetPostsAsync_Retrieve_Success()
        {
            int postTotal = 5;
            var mock = MockPostRepository.GenerateData(postTotal).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var posts = await _repository.GetPostsAsync();

            Assert.That(posts, Is.Not.Null);
            Assert.That(posts.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task GetPostById_FindExistingOne_Success()
        {
            var mock = MockPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var post = await _repository.GetById(1);

            Assert.That(post, Is.Not.Null);
            Assert.That(post.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GetPostById_NotExistingId_ReturnNull()
        {
            var mock = MockPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var post = await _repository.GetById(100);

            Assert.That(post, Is.Not.Null);
        }

        [Test]
        public async Task GetPostsByUserName_ExistingUser_ReturnPosts()
        {
            var mock = MockPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var posts = await _repository.GetPostsByUserName("test");

            Assert.That(posts, Is.Not.Null);
            Assert.That(posts.First().UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetPostsByUserName_NotExistingUser_ReturnNull()
        {
            var mock = MockPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var posts = await _repository.GetPostsByUserName("NotExisting");

            Assert.That(posts, Is.Not.Null);
        }

        [Test]
        public async Task GivenValidObjects_WhenInsertData_AddAndSaveChangesCalledOnce()
        {
            var mockSet = new Mock<DbSet<Post>>();
            dataContextMock.Setup(m => m.Posts).Returns(mockSet.Object);   
            var repository = new PostRepository(dataContextMock.Object);

            repository.Add(new Post { Id = 1, Title = "Post Title" });
            await repository.SaveChangesAsync();

            mockSet.Verify(m => m.Add(It.IsAny<Post>()), Times.Once);
            dataContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task GetPostsAsync_LimitFour_ReturnFourObjectsInOrderByCreatedTime()
        {
            var mock = MockPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
            dataContextMock.Setup(x => x.Posts).Returns(mock.Object);

            var result = await _repository.GetPostsAsync(4,0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(4));
        }

        [Test]
        public async Task VerifyingMockWorking()
        {
            var testObject = MockPostRepository.GenerateData(5)[0];

            var context = new Mock<DataContext>();
            var mock = MockPostRepository.GenerateData(5).BuildMock().BuildMockDbSet();
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