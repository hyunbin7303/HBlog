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

        private readonly IPostService _postService;
        private readonly IMapper _mapper;


        public PostsController(IPostService postService, IPostRepository postRepository, IMapper mapper)
        {
            _mapper = mapper;
            _postService = postService;
            _postRepository = postRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPosts() => Ok(await _postService.GetPosts());

        [AllowAnonymous]
        [HttpGet("users/{username}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUsername(string username) => Ok(await _postRepository.GetPostsByUserName(username));

        [AllowAnonymous]
        [HttpGet("Tags/{tagName}")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPostsByTagName(string tagName) => Ok(await _postService.GetPostsByTagName(tagName));


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDisplayDetailsDto>> GetPostById(int id)
        {
            var postDetails = await _postService.GetByIdAsync(id);
            if (postDetails.IsSuccess)
                return Ok(postDetails.Value);
            else
            {
                return BadRequest(postDetails.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PostUpdateDto postUpdateDto)
        {
            if(postUpdateDto is null)
                throw new ArgumentNullException(nameof(postUpdateDto));

            var result = await _postService.UpdatePost(postUpdateDto);
            if (!result.IsSuccess && result.Message == "Post does not exist.")
            {
                RedirectToRoute("Posts");
            }
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
        public async Task<ActionResult<string>> Create(PostCreateDto postCreateDto)
        {
            if (postCreateDto is null)
                throw new ArgumentNullException(nameof(postCreateDto));

            var result = await _postService.CreatePost(User.GetUsername(), postCreateDto);
            return Ok(result.Message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _postRepository.Remove(id);
            return Ok();
        }

    }
}
