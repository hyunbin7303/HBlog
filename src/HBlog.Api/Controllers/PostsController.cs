using HBlog.Application.Commands.Posts;
using HBlog.Application.Queries.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HBlog.Domain.Entities;
using HBlog.Domain.Repositories;
using HBlog.Infrastructure.Extensions;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common.Params;
using MediatR;


namespace HBlog.Api.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
	    private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
		public PostsController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPosts([FromQuery]PostParams queryParams)
        {
			return Ok(new ApiResponse<IEnumerable<PostDisplayDto>>(await _mediator.Send(new GetPostsQuery(queryParams))));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("categories/{categoryId}/posts")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPostsByCategory(int categoryId)
        {
	        var result = await _mediator.Send(new GetPostsByCategoryQuery(categoryId));
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

			return Ok(await _mediator.Send(new GetPostsByUsernameQuery(user.UserName)));
        }

        [AllowAnonymous]
        [HttpGet("tags/{tagId}/posts")]
        public async Task<ActionResult<IEnumerable<PostDisplayDto>>> GetPostsbyTagId(int tagId)
        {
			return Ok(await _mediator.Send(new GetPostsByTagIdQuery(tagId)));
        }

        [AllowAnonymous]
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<PostDisplayDetailsDto>> GetPostById(int id)
        {
			var postDetails = await _mediator.Send(new GetPostByIdQuery(id));
            return postDetails.IsSuccess ? 
                    Ok(postDetails.Value) : 
                    NotFound(postDetails.Message);
        }

        [AllowAnonymous]
        [HttpGet("posts/title-contains")]
        public async Task<ActionResult<PostDisplayDto>> GetPostsByTitleContains(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return BadRequest("Search title string cannot be empty.");
            var posts = await _mediator.Send(new GetPostsTitleContainsQuery(title));
            return posts.IsSuccess
                ? Ok(posts.Value)
                : NotFound(posts.Message);
        }

        [HttpPut("posts")]
        public async Task<IActionResult> Put(PostUpdateDto postUpdateDto)
        {
            if(postUpdateDto is null)
                return BadRequest($"Argument null for {nameof(postUpdateDto)}.");

            if (postUpdateDto.Id == 0)
                return BadRequest("Id field cannot be empty or 0");

            var result = await _mediator.Send(new UpdatePostCommand(postUpdateDto));
            if (!result.IsSuccess && result.Message == "Post does not exist.")
                RedirectToRoute("Posts");

            return NoContent();
        }

        [HttpPut("posts/{postId}/status")]
        public async Task<IActionResult> ChangeStatus(int postId, [FromBody] PostChangeStatusDto statusDto)
        {
            if (postId == 0)
                return BadRequest("Post Id cannot be null");

            var result = await _mediator.Send(new UpdatePostStatusCommand(postId, statusDto));
            if (!result.IsSuccess)
                return BadRequest("Failed to update status.");

            return Ok();
        }

        [HttpPut("posts/{postId}/Tags")] 
        public async Task<IActionResult> AddTag(int postId, [FromBody]int[] tagId)
        {
            if (postId == 0 || tagId.Length == 0)
                return BadRequest("Post Id or Tag Id cannot be null");

            var result = await _mediator.Send(new AddTagForPostCommand(postId, tagId));
            if(!result.IsSuccess)
                return BadRequest("Failed to add tags.");
            
            return Ok();
        }

        [HttpPost("posts")]
        public async Task<IActionResult> Create(PostCreateDto postCreateDto)
        {
            if (postCreateDto is null)
                return BadRequest($"Argument null for {nameof(postCreateDto)}.");

            var result = await _mediator.Send(new CreatePostCommand(User.GetUsername(), postCreateDto));

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.IsSuccess);
        }

        [HttpDelete("posts/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeletePostCommand(id));
            if(!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok();
        }

    }
}
