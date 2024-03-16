using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Helpers;
using Moq;

namespace KevBlog.UnitTests.Services
{
    public class LikeServiceTest : TestBase
    {
        private ILikeService _likeService;
        private readonly Mock<ILikesRepository> _likeRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        public LikeServiceTest()
        {
            _likeService = new LikeService(_mapper, _likeRepoMock.Object, _userRepoMock.Object);
        }

    }
}
