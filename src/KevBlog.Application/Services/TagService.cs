using AutoMapper;
using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace KevBlog.Application.Services
{
    public class TagService : BaseService, ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IPostRepository _postRepository;
        private readonly IRepository<PostTags> _postTagRepository;
        public TagService(IMapper mapper, ITagRepository tagRepository, IPostRepository postRepository, IRepository<PostTags> postTagRepository) : base(mapper)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
        }
        public async Task<ServiceResult> AddTagToPost(int postId, int tagId)
        {
            var post = await _postRepository.GetById(postId);
            if (post is null)
                return ServiceResult.Fail(msg: $"Post Id:{postId} does not exist.");

            var tag = await _tagRepository.GetById(tagId);
            if (tag is null)
                return ServiceResult.Fail(msg: $"Tag Id:{tagId} does not exist.");

            _postTagRepository.Add(new PostTags { PostId = post.Id, TagId = tag.Id });
            var result = await _postTagRepository.SaveChangesAsync();
            return ServiceResult.Success($"Successfully created the tag name : {tag.Name}");
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
            return _mapper.Map<IList<TagDto>>(tags);
        }

        public async Task<ServiceResult<IEnumerable<TagDto>>> GetTagsByPostId(int postId)
        {
            var post = await  _postRepository.GetById(postId);
            if (post is null)
                return ServiceResult.Fail<IEnumerable<TagDto>>(msg: $"Post Id:{postId} does not exist.");

            var result = _postTagRepository.GetAll();
            var tags = result.Include(x=> x.Post).Include(x=> x.Tag).Where(p => p.PostId == postId).Select(o => o.Tag).ToList();
            var resultTag = _mapper.Map<IEnumerable<TagDto>>(tags);
            return ServiceResult.Success(resultTag);
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
    