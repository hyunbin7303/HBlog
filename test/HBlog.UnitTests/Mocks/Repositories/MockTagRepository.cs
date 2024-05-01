using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using Moq;

namespace HBlog.UnitTests.Mocks.Repositories
{
    public class MockTagRepository : Mock<ITagRepository>
    {
        public MockTagRepository MockgetTagById(Tag tag)
        {
            Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(tag);
            return this;
        }
    }
}
