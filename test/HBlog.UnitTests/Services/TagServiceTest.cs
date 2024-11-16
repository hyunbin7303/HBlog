using HBlog.Application.Services;
using HBlog.Contract.DTOs;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.TestUtilities;
using Moq;
using NUnit.Framework;

namespace HBlog.UnitTests.Services
{
    public class TagServiceTest : TestBase
    {
        private ITagService _tagService;
        private readonly Mock<ITagRepository> _tagRepositoryMock = new();
        private readonly Mock<IPostRepository> _postRepositoryMock = new();
        public TagServiceTest()
        {
            _tagService = new TagService(_mapper, _tagRepositoryMock.Object, _postRepositoryMock.Object, null);
        }
        [Test]
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.First().TagId, Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Programming"));
        }

        [Test]
        public async Task GivenTitleEmpty_WhenCreateTagcalled_ThenReturnMessage()
        {
            var result = await _tagService.CreateTag(new TagCreateDto { });

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Tag Title is empty."));
        }

        [Test]
        public async Task GivenValidTag_WhenCreateTag_ThenReturnSuccess()
        {
            TagCreateDto tagDto = new TagCreateDto { Desc = "test", Name = "Tagname", Slug = "TestingSlug" };
            _tagRepositoryMock.Setup(o => o.Add(It.IsAny<Tag>()));  

            var result = await _tagService.CreateTag(tagDto);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Message, Is.EqualTo($"Successfully created the tag name : {tagDto.Name}"));
            _tagRepositoryMock.Verify(o => o.Add(It.IsAny<Tag>()));
        }

        [Test] 
        public async Task GivenInvalidTagId_WhenDeleteTagCalled_ThenReturnErrorMessage()
        {
            int wrongTagId = 100;

            var result = await _tagService.RemoveTag(wrongTagId);

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo($"Tag id:{wrongTagId} is not exist."));
            _tagRepositoryMock.Verify(x => x.Delete(It.IsAny<Tag>()), Times.Never);
        }
        [Test]
        public async Task GivenValidTagId_WhenDeleteTagCalled_ThenReturnSuccess()
        {
            int validTagId = 5;
            _tagRepositoryMock.Setup(o => o.GetById(validTagId)).ReturnsAsync(new Tag { Id = validTagId, Name = "ValidTag",  Slug = "ValidTag" });
      
            var result = await _tagService.RemoveTag(validTagId);

            Assert.That(result.IsSuccess, Is.True);
        }
    }
}
