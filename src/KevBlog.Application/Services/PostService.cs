using AutoMapper;
using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Constants;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;

namespace KevBlog.Application.Services
{
    public class PostService : BaseService, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;   
        public PostService(IMapper mapper, IPostRepository postRepository, IUserRepository userRepository) : base(mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public Task AddTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> CreatePost(string userName, PostCreateDto createDto)
        {
            if (string.IsNullOrEmpty(createDto.Title))
                return ServiceResult.Fail(msg: "Title cannot be empty.");

            var user = await _userRepository.GetUserByUsernameAsync(userName);
            var post = _mapper.Map<Post>(createDto);
            post.User = user;
            post.UserId = user.Id;
            await _postRepository.CreateAsync(post);
            return ServiceResult.Success(msg: "Success to create post");
        }

        public async Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id)
        {
            Post post = await _postRepository.GetPostById(id);
            if (post is null || post.Status is PostStatus.Removed)
                return ServiceResult.Fail<PostDisplayDetailsDto>(msg: "Post is not exist or status is removed.");

            User user = await _userRepository.GetUserByIdAsync(post.UserId);
            var postDisplay = _mapper.Map<PostDisplayDetailsDto>(post);
            postDisplay.UserName = user?.UserName ?? "Unknown";
            return ServiceResult.Success(postDisplay);
        }

        public async Task<IEnumerable<PostDisplayDto>> GetPosts()
        {
            IEnumerable<Post> posts = await _postRepository.GetPostsAsync();
            var postDisplays = _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
            return postDisplays;
        }

        public async Task<IEnumerable<PostDisplayDto>> GetPostsByTagName(string tagName)
        {
            IEnumerable<Post> posts = await _postRepository.GetPostsAsync();
            var postDisplays = _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
            return postDisplays;

        }

        public async Task<ServiceResult> UpdatePost(PostUpdateDto updateDto)
        {
            var post = await _postRepository.GetPostById(updateDto.Id);
            if (post == null || post.Status == PostStatus.Removed)
                return ServiceResult.Fail(msg: "Post does not exist.");

            post.Desc = updateDto.Desc;
            post.Content = updateDto.Content;
            post.Type = updateDto.Type;
            post.LinkForPost = updateDto.LinkForPost;
            await _postRepository.UpdateAsync(post);
            return ServiceResult.Success();
        }
    }
}
