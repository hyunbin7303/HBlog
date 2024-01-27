using KevBlog.Application.Services;
using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
using Moq;
namespace KevBlog.UnitTests.Mocks.Services
{
    public class MockPostsService : Mock<IPostService>
    {
        public MockPostsService MockGetById(ServiceResult<PostDisplayDetailsDto> result)
        {
            Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(result);
            return this;
        }
        public MockPostsService MockGetByIdInvalid(ServiceResult<PostDisplayDetailsDto> result)
        {
            Setup(x => x.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("Need to setup?"));
            return this;
        }

        public MockPostsService MockVerifyGetAll(Times times)
        {
            Verify(x => x.GetPosts(), times);
            return this;
        }
    }
}
