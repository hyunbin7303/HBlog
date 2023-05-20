using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Common;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace KevBlog.Api.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILikeService _likeService;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository, ILikeService likeService)
        {
            _likeService = likeService;
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){
            var sourceUserId = User.GetUserId();
            var result = await _likeService.AddLike(sourceUserId, username);
            if(!result.IsSuccess && result.Message == "NotFound")
                return NotFound(result.Message);
            if (!result.IsSuccess)
                return BadRequest("Failed to like user");

            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<PageList<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParam) {
            likesParam.UserId = User.GetUserId();
            var users = await _likeService.GetUserLikePageList(likesParam);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
            return Ok(users);
        }
    }
}