using KevBlog.Application.Services;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KevBlog.UnitTests.Services
{
    public class TagServiceTest : ServiceTest
    {
        private ITagService _tagService;
        private readonly Mock<ITagRepository> _tagRepositoryMock = new();
        public TagServiceTest()
        {
            _tagService = new TagService(_mapper, _tagRepositoryMock.Object);
        }
        [Fact]
        public async Task WhenGetAllTagsCalled_ThenReturnAllTags()
        {
            var result = await _tagService.GetAllTags();

            Assert.NotNull(result);
        }
    }
}
