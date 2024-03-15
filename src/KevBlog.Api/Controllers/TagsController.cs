using KevBlog.Application.Services;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KevBlog.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TagsController : BaseApiController
    {
        private readonly ITagService _tagService;
        private readonly IUserService _userService;
        public TagsController(ITagService tagService, IUserService userService)
        {
            this._tagService = tagService;
            this._userService = userService;
        }
        [HttpPost]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tagService.RemoveTag(id);
            return Ok(result.IsSuccess);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get() => Ok(await _tagService.GetAllTags());
    }
}
