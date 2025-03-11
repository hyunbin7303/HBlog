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
    public class PostRepositoryTest : IDisposable
    {
        private readonly DataContext _context;
        private IPostRepository _repository;       
        public PostRepositoryTest()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestingPostRepo").Options;

            _context = new DataContext(dbContextOptions);
            _repository = new PostRepository(_context);

            IEnumerable<Post> posts = MockPostRepository.GenerateData(5);
            _context.Posts.AddRange(posts);
            _context.SaveChanges();
        }


        [Test]  
        public async Task GetPostsAsync_Retrieve_Success()
        {
            var posts = await _repository.GetPostsAsync();

            Assert.That(posts, Is.Not.Null);
            Assert.That(posts.Count(), Is.EqualTo(5));
        }

        [Test]
        public async Task GetPostById_FindExistingOne_Success()
        {
            var post = await _repository.GetById(1);

            Assert.That(post, Is.Not.Null);
            Assert.That(post.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task GivenInvalidId_WhenGetById_ReturnNull()
        {
            var post = await _repository.GetById(100);

            Assert.That(post, Is.Null);
        }

        [Test]
        public async Task GetPostsByUserName_ExistingUser_ReturnPosts()
        {
            var posts = await _repository.GetPostsByUserName("test");

            Assert.That(posts, Is.Not.Null);
            //Assert.That(posts.First().UserId, Is.Not.Null);
        }

        [Test]
        public async Task GetPostsByUserName_NotExistingUser_ReturnEmpty()
        {
            var posts = await _repository.GetPostsByUserName("NotExisting");

            Assert.That(posts, Is.Empty);
        }

        [Test]
        public async Task GetPostsAsync_LimitFour_ReturnFourObjectsInOrderByCreatedTime()
        {
            var result = await _repository.GetPostsAsync(4,0);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(4));
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}