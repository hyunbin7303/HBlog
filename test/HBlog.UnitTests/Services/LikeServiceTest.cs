using HBlog.Application.Services;
using HBlog.Domain.Entities;
using HBlog.Domain.Params;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Helpers;
using HBlog.TestUtilities;
using Moq;

namespace HBlog.UnitTests.Services
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
