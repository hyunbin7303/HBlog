using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

namespace KevBlog.UnitTests.Mocks.Repositories
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
