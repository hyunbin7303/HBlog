using KevBlog.Application.Services;
using KevBlog.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevBlog.UnitTests.Services
{
    public class PostServiceTest : ServiceTest
    {
        private IPostService _postService;
        private Mock<IPostRepository> _postRepositoryMock = new();
        private Mock<IUserRepository> _UserRepositoryMock = new();

        public PostServiceTest()
        {
            _postService = new PostService(_mapper, _postRepositoryMock.Object, _UserRepositoryMock.Object);
        }
        [Fact]
        public async Task GetPostsByUsername_NotExistingName_ReturnFalse()
        {
            string userName = "NotExisting";

            var posts = await _postService.GetPostsByUsername(userName);

            Assert.Null(posts);
        }

        [Fact]
        public async Task GetPostsByUsername_ExistingUserName_ReturnPosts()
        {
            string userName = "kevin0";

            var posts = await _postService.GetPostsByUsername(userName);

            Assert.NotNull(posts);
        }
    }
}
