using AutoMapper;
using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
namespace KevBlog.Application.Services
{
    public class TagService : BaseService, ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService(IMapper mapper, ITagRepository tagRepository) : base(mapper)
        {
            _tagRepository = tagRepository;
        }
        public Task<ServiceResult> AddTagToPost(int postId, string tagName)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> CreateTag(TagCreateDto tag)
        {
            if (tag.Name == null)
                return ServiceResult.Fail(msg: "Tag Title is empty.");

            _tagRepository.Add(new Tag { Name = tag.Name, Slug = tag.Slug, Desc = tag.Desc });
            return ServiceResult.Success($"Successfully created the tag name : {tag.Name}");
        }

        public async Task<IEnumerable<TagDto>> GetAllTags()
        {
            var tags = await _tagRepository.GetAll();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<ServiceResult> RemoveTag(int tagId)
        {
            var tag = await _tagRepository.GetById(tagId);
            if(tag is null)
                return ServiceResult.Fail(msg: $"Tag id:{tagId} is not exist.");

            _tagRepository.Remove(tagId);
            return ServiceResult.Success();
        }
    }
}
    