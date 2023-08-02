using AutoMapper;
using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
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
            if(tag.Name == null)
                return ServiceResult.Fail(msg: "Tag Title is empty.");
            
            var newTag = new Tag { Name = tag.Name, Slug = tag.Slug, Desc = tag.Desc };
            await _tagRepository.Insert(newTag);
            return ServiceResult.Success();
        }

        public async Task<IEnumerable<TagDto>> GetAllTags()
        {
            var tags = await _tagRepository.GetAll();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public Task<ServiceResult> RemoveTag(int tagId)
        {
            throw new NotImplementedException();
        }
    }
}
