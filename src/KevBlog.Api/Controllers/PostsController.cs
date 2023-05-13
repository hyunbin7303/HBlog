using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KevBlog.Domain.Entities;
using KevBlog.Application.DTOs;
using AutoMapper;
using KevBlog.Domain.Constants;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Application.Services;

namespace KevBlog.Api.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        private readonly IPostService _postService;
        private readonly IMapper _mapper;


        public PostsController(IPostService postService, IPostRepository postRepository, IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _postService = postService;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPosts() => Ok(await _postService.GetPosts());

        [AllowAnonymous]
        [HttpGet("users/{username}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUsername(string username)
        {
            return Ok(await _postRepository.GetPostsByUserName(username));
        }

        [AllowAnonymous]
        [HttpGet("Tags/{tagName}")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPostsByTagName(string tagName)
        {
            var posts = await _postRepository.GetPostsByTagname(tagName);
            if (posts is null) return NotFound();

            var postDisplays = _mapper.Map<IEnumerable<PostDisplayDto>>(posts);
            return Ok(postDisplays);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDisplayDetailsDto>> GetPostById(int id)
        {

            var postDetails = await _postService.GetByIdAsync(id);

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
            if (postCreateDto is null)
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
