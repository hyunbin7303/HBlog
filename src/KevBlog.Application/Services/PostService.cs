using AutoMapper;
using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Common.Params;
using KevBlog.Domain.Constants;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace KevBlog.Application.Services;
public class PostService : BaseService, IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IRepository<PostTags> _postTagRepository;
    private readonly ICategoryRepository _categoryRepository;
    public PostService(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository, ITagRepository tagRepository, IRepository<PostTags> postTagRepository, ICategoryRepository categoryRepository) : base(mapper)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _tagRepository = tagRepository;
        _postTagRepository = postTagRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ServiceResult> AddTagForPost(int postId, int tagId)
    {
        var post = await _postRepository.GetById(postId);
        if(post is null)
            return ServiceResult.NotFound(msg: "Cannot find post.");

        var tag = await _tagRepository.GetById(tagId);
        if (tag is null)
            return ServiceResult.NotFound(msg: "Cannot find tag."); 

        var postTags = new PostTags { PostId = post.Id, TagId = tag.Id };
        _postTagRepository.Add(postTags);
        await _postTagRepository.SaveChangesAsync();
        return ServiceResult.Success($"Success to add Tag ID: {tagId}");
    }

    public async Task<ServiceResult> AddCategoryForPost(int postId, int categoryId)
    {
        var post = await _postRepository.GetById(postId);
        if (post is null)
            return ServiceResult.Fail(msg: "Post Id is not valid.");

        var category = await _categoryRepository.GetById(categoryId);
        if (category is null)
            return ServiceResult.Fail(msg: "Category Id is not valid.");

        //TODO Update Category?
        return ServiceResult.Success($"Success to add Category ID: {categoryId}");
    }

    public async Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto)
    {
        if (string.IsNullOrEmpty(createDto.Title))
            return ServiceResult.Fail(msg: "Title cannot be empty.");

        if (createDto.CategoryId == 0)
            return ServiceResult.Fail(msg: "Category needs to be setup.");

        var category = await _categoryRepository.GetById(createDto.CategoryId);
        if (category is null)
            return ServiceResult.NotFound(msg: "Cannot find category.");

        var user = await _userRepository.GetUserByUsernameAsync(userName);
        var post = _mapper.Map<Post>(createDto);
        post.User = user;
        post.UserId = user.Id;
        post.Status = PostStatus.Active;
        _postRepository.Add(post);
        await _postRepository.SaveChangesAsync();
        return ServiceResult.Success(msg: $"Post Id:{post.Id}");
    }

    public async Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id)
    {
        Post post = await _postRepository.GetPostDetails(id);
        if (post is null || post.Status is PostStatus.Removed)
            return ServiceResult.Fail<PostDisplayDetailsDto>(msg: "Post is not exist or status is removed.");

        var postDisplay = _mapper.Map<PostDisplayDetailsDto>(post);

        User user = await _userRepository.GetUserByIdAsync(post.UserId);
        postDisplay.UserName = user?.UserName ?? "Unknown";
      
        return ServiceResult.Success(postDisplay);
    }

    public async Task<IEnumerable<PostDisplayDto>> GetPosts(PostParams query)
    {
        IEnumerable<Post> posts = await _postRepository.GetPostsAsync(query.Limit, query.Offset);
        if(query.CategoryId != 0)
            posts = posts.Where(p =>p.CategoryId == query.CategoryId);

        if (query.TagIds.Any())
            posts = posts.Where(p => p.PostTags.Any(tag => query.TagIds.Contains(tag.TagId)));

        return _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByTagSlug(string tagSlug)
    {
        var tags = await _tagRepository.FindbySlug(tagSlug);
        if(tags is null)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "Tag does not exist.");
        
        var tagPosts = await _postTagRepository.GetAll(o => o.TagId == tags.Id);
        var posts = tagPosts.Select(o => o.Post);
        var result =  _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
        return ServiceResult.Success(result);

    }

    public async Task<ServiceResult> UpdatePost(PostUpdateDto updateDto)
    {
        Post post = await _postRepository.GetById(updateDto.Id);
        if (post == null || post.Status == PostStatus.Removed)
            return ServiceResult.Fail(msg: "Post does not exist.");

        post.Title = updateDto.Title;
        post.Desc = updateDto.Desc;
        post.Content = updateDto.Content;
        post.Type = updateDto.Type;
        post.LinkForPost = updateDto.LinkForPost;
        post.LastUpdated = DateTime.UtcNow;

        await _postRepository.UpdateAsync(post);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeletePost(int id)
    {
        var post = await _postRepository.GetById(id);
        if (post is null)
            return ServiceResult.Fail(msg: "NotFound");

        _postRepository.Remove(id);
        await _postRepository.SaveChangesAsync();
        return ServiceResult.Success(msg: $"Removed Post Id: {id}");
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByCategory(int categoryId)
    {
        var category = await _categoryRepository.GetById(categoryId);
        if(category is null)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "NotFound Category.");

        var posts = await _postRepository.GetAll(o => o.CategoryId == categoryId);
        if(posts.Count() == 0)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "NotFound Posts.");

        return ServiceResult.Success(_mapper.Map<IEnumerable<PostDisplayDto>>(posts));
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByUsername(string userName)
    {
        var posts = await _postRepository.GetPostsByUserName(userName);
        return ServiceResult.Success(_mapper.Map<IEnumerable<PostDisplayDto>>(posts));
    }

    public Task<ServiceResult<PostDisplayDetailsDto>> GetBySlugAsync(string slug)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResult<IEnumerable<PostDisplayDto>>> GetPostsByTagId(int tagId)
    {
        var tag = await _tagRepository.GetById(tagId);
        if (tag is null)
            return ServiceResult.Fail<IEnumerable<PostDisplayDto>>(msg: "NotFound Tag.");

        var postTags = _postTagRepository.GetAll();
        var posts = postTags.Include(o => o.Post).Where(t => t.TagId == tagId).Select(x => x.Post);
        return ServiceResult.Success(_mapper.Map<IEnumerable<PostDisplayDto>>(posts));
    }
}
