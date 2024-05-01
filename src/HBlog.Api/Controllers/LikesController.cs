using HBlog.Application.Services;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Params;
using HBlog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HBlog.Api.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("likes/{username}")]
        public async Task<ActionResult> AddLike(string username){
            var sourceUserId = User.GetUserId();
            var result = await _likeService.AddLike(sourceUserId, username);
            if(!result.IsSuccess && result.Message == "NotFound")
                return NotFound(result.Message);
            if (!result.IsSuccess)
                return BadRequest("Failed to like user");

            return Ok(result);
        }
        [HttpGet("likes")]
        public async Task<ActionResult<PageList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParam) {
            likesParam.UserId = User.GetUserId();
            var users = await _likeService.GetUserLikePageList(likesParam);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
    }
}