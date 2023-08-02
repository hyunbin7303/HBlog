using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Moq;

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
            IEnumerable<Tag> tags = new List<Tag>()
            {
                new Tag { Id = 1, Name = "Programming", Desc = "Programming Desc", Slug = "SlugTest1"},
                new Tag { Id = 2, Name = "Shopping", Desc = "", Slug = "SlugTest2"},
                new Tag { Id = 3, Name = "dotnet", Desc = "", Slug = "SlugTest3"},
            };
            _tagRepositoryMock.Setup(o => o.GetAll()).ReturnsAsync(tags);
            var result = await _tagService.GetAllTags();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Equal(1, result.First().TagId);
            Assert.Equal("Programming", result.First().Name);
        }

        [Fact]
        public async Task GivenTitleEmpty_WhenCreateTagcalled_ThenReturnMessage()
        {
            var result = await _tagService.CreateTag(new TagCreateDto { });

            Assert.False(result.IsSuccess);
            Assert.Equal("Tag Title is empty.", result.Message);
        }

        [Fact]
        public async Task GivenValidTag_WhenCreateTag_ThenReturnSuccess()
        {
            _tagRepositoryMock.Setup(o => o.Insert(It.IsAny<Tag>())).Returns(Task.FromResult(new Tag()));  
            TagCreateDto tagDto = new TagCreateDto { Desc = "test", Name = "Tagname", Slug = "TestingSlug" };

            var result = await _tagService.CreateTag(tagDto);

            _tagRepositoryMock.Verify(o => o.Insert(It.IsAny<Tag>()));
            Assert.True(result.IsSuccess);
        }

        [Fact] // TODO Need to be implemented first.
        public async Task GivenInvalidTagId_WhenDeleteTagCalled_ThenReturnErrorMessage()
        {
            int wrongTagId = 100;

            var result = await _tagService.RemoveTag(wrongTagId);

        }
    }
}
