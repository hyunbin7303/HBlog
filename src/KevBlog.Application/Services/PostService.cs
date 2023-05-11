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

        public async Task<ServiceResult<PostDisplayDetailsDto>> GetByIdAsync(int id)
        {
            Post post = await _postRepository.GetPostById(id);
            if (post is null || post.Status is PostStatus.Removed)
                return ServiceResult.Fail<PostDisplayDetailsDto>(msg: "Post is not exist or status is removed.");

            User user = await _userRepository.GetUserByIdAsync(post.UserId);
            var postDisplay = _mapper.Map<PostDisplayDetailsDto>(post);
            postDisplay.UserName = user.UserName ?? null;
            return ServiceResult.Success(postDisplay);
        }

        public async Task<IEnumerable<PostDisplayDto>> GetPosts()
        {
            IEnumerable<Post> posts = await _postRepository.GetPostsAsync();
            var postDisplays = _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
            return postDisplays;
        }

        public Task<IEnumerable<Post>> GetPostsByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
