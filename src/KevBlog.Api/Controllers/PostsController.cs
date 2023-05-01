using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KevBlog.Domain.Entities;
using KevBlog.Application.DTOs;
using AutoMapper;
using KevBlog.Domain.Constants;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;

namespace KevBlog.Api.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPosts()
        {
            IEnumerable<Post> posts = await _postRepository.GetPostsAsync();
            var postDisplays = _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
            return Ok(postDisplays);
        }
        [AllowAnonymous]
        [HttpGet("users/{username}")]
        public async Task<ActionResult<Post>> GetPostByUsername(string username)
        {
            return await _postRepository.GetPostByUsername(username);
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDisplayDetailsDto>> GetPostById(int id)
        {
            Post post = await _postRepository.GetPostById(id);
            if(post is null || post.Status is PostStatus.Removed) 
                return NotFound();

            User user = await _userRepository.GetUserByIdAsync(post.UserId);
            var postDisplay = _mapper.Map<PostDisplayDetailsDto>(post);
            postDisplay.UserName = user.UserName ?? null;
            return Ok(postDisplay);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PostUpdateDto postUpdateDto)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null || post.Status == PostStatus.Removed)
                return NotFound(); // or should be RedirectToRoute("Homepage");

            post.Desc = postUpdateDto.Desc;
            post.Content = postUpdateDto.Content;
            post.Type = postUpdateDto.Type;
            post.LinkForPost = postUpdateDto.LinkForPost;

            await _postRepository.UpdateAsync(post);
            return NoContent();
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> SetPostStatus(int id, string status)
        {
            Post post = await _postRepository.GetPostById(id);
            if (post is null)
                return NotFound();

            post.Status = status;
            await _postRepository.UpdateAsync(post);
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult<User>> Create(PostCreateDto postCreateDto)
        {
            if (postCreateDto == null)
                throw new ArgumentNullException(nameof(postCreateDto));

            if (string.IsNullOrEmpty(postCreateDto.Title))
                return BadRequest("Title cannot be empty.");

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var post = _mapper.Map<Post>(postCreateDto);
            post.User = user;
            post.UserId = user.Id;
            await _postRepository.CreateAsync(post);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _postRepository.Remove(id);
            return Ok();
        }

    }
}
