using KevBlog.Application.Services;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KevBlog.Api.Controllers
{
    [Authorize]
    public class TagsController : BaseApiController
    {
        private readonly ITagService _tagService;
        private readonly IUserService _userService;
        public TagsController(ITagService tagService, IUserService userService)
        {
            _tagService = tagService;
            _userService = userService;
        }
        [HttpPost("tags")]
        public async Task<ActionResult> Create(TagCreateDto tagCreateDto)
        {
            if (tagCreateDto is null)
                throw new ArgumentNullException(nameof(tagCreateDto));

            if (string.IsNullOrEmpty(tagCreateDto.Name))
                return BadRequest("Tag Name cannot be empty.");

            var user = await _userService.GetMembersByUsernameAsync(User.GetUsername());
            if (user.Value is null) return NotFound();

            var result = await _tagService.CreateTag(tagCreateDto);
            return Ok(result.Message);
        }

        [HttpDelete("tags/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tagService.RemoveTag(id);
            return Ok(result.IsSuccess);
        }

        [AllowAnonymous]
        [HttpGet("tags")]
        public async Task<ActionResult<IEnumerable<Tag>>> Get() => Ok(await _tagService.GetAllTags());

        [AllowAnonymous]
        [HttpGet("posts/{postId}/tags")]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTagByPostId(int postId)
        {
            var result = await _tagService.GetTagsByPostId(postId);
            return Ok(result);
        }
    }
}