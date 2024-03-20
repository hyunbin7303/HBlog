using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Contract.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Params;
using KevBlog.Domain.Common.Params;


namespace KevBlog.Api.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        private readonly IPostService _postService;
        private readonly IUserRepository _userRepository;
        public PostsController(IPostService postService, IUserRepository userRepository)
        {
            _postService = postService;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPosts([FromQuery]QueryParams queryParams)
        {
            return Ok(new ApiResponse<IEnumerable<PostDisplayDto>>(await _postService.GetPosts(queryParams)));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("categories/{categoryId}/posts")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPostsByCategory(int categoryId)
        {
            var result = await _postService.GetPostsByCategory(categoryId);
            if(result.IsSuccess is false)
                return NotFound(result.Message);  
            
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpGet("users/{username}/posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUsername(string username) 
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is null)
                return NotFound("User not found.");

            return Ok(await _postService.GetPostsByUsername(user.UserName));
        }


        [AllowAnonymous]
        [HttpGet("tags/{tagName}/posts")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPostsByTagName(string tagName) => Ok(await _postService.GetPostsByTagName(tagName));

        [AllowAnonymous]
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<PostDisplayDetailsDto>> GetPostById(int id)
        {
            var postDetails = await _postService.GetByIdAsync(id);
            return (await _postService.GetByIdAsync(id)).IsSuccess ? 
                    (ActionResult<PostDisplayDetailsDto>)Ok(postDetails.Value) : 
                    (ActionResult<PostDisplayDetailsDto>)NotFound(postDetails.Message);
        }

        [HttpPut("posts")]
        public async Task<IActionResult> Put(PostUpdateDto postUpdateDto)
        {
            if(postUpdateDto is null)
                return BadRequest($"Argument null for {nameof(postUpdateDto)}.");

            if (postUpdateDto.Id == 0)
                return BadRequest("Id field cannot be empty or 0");

            var result = await _postService.UpdatePost(postUpdateDto);
            if (!result.IsSuccess && result.Message == "Post does not exist.")
                RedirectToRoute("Posts");

            return NoContent();
        }

        [HttpPut("posts/{postId}/AddTag")] 
        public async Task<IActionResult> AddTag(int postId, [FromBody]int tagId)
        {
            if (postId == 0 || tagId == 0)
                return BadRequest("Post Id or Tag Id cannot be null");
            
            var result = await _postService.AddTagForPost(postId, tagId);
            if(!result.IsSuccess)
                return BadRequest("Failed to add tags.");
            
            return Ok();
        }

        [HttpPost("posts")]
        public async Task<IActionResult> Create(PostCreateDto postCreateDto)
        {
            if (postCreateDto is null)
                throw new ArgumentNullException(nameof(postCreateDto));

            var result = await _postService.CreatePost(User.GetUsername(), postCreateDto);
            if(!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.IsSuccess);
        }

        [HttpDelete("posts/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _postService.DeletePost(id);
            if(!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }

    }
}
