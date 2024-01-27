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
            _likeService = new LikeSerivce(_mapper, _likeRepoMock.Object, _userRepoMock.Object);
        }

        [Fact]
        public async Task Given_When_Then()
        {
            var result = await _likeService.AddLike(1, "hyunbin7303");
        }

        [Fact]
        public async Task Given_When_Then2()
        {
            LikesParams likesParams = new();
            var result = await _likeService.GetUserLikePageList(likesParams);

            //Assert.False(result.)
        }
    }
}
