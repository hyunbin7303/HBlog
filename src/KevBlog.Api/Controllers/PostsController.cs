using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Interfaces;
using KevBlog.Application.DTOs;
using AutoMapper;
using System.Security.Claims;
using KevBlog.Domain.Constants;
using KevBlog.Infrastructure.Repositories;

namespace API.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PostsController(IPostRepository postRepository, IUserRepository userRepository, IMapper mapper)
        {
            this._postRepository = postRepository;
            this._mapper = mapper;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return Ok(await _postRepository.GetPostsAsync());
        }

        [AllowAnonymous]
        [HttpGet("users/{username}")]
        public async Task<ActionResult<Post>> GetPostByUsername(string username)
        {
            return await _postRepository.GetPostByUsername(username);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            return await _postRepository.GetPostById(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PostUpdateDto postUpdateDto)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null || post.Status == PostStatus.Removed)
                return NotFound(); // or should be RedirectToRoute("Homepage");

            var updatedPost = _mapper.Map<Post>(postUpdateDto);
            await _postRepository.UpdateAsync(post);
            return Ok();
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
            if(postCreateDto == null)
                throw new ArgumentNullException(nameof(postCreateDto));

            if (string.IsNullOrEmpty(postCreateDto.Title))
                return BadRequest("Title cannot be empty.");

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

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
