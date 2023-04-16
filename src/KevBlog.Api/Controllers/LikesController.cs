using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Api.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository, IMapper mapper)
        {
            _mapper = mapper;
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){
            var sourceUserId = int.Parse(User.GetUserId());
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);
            if(likedUser == null) return NotFound();

            if(sourceUser.UserName == username) return BadRequest("You cannot like yourself.");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            if(userLike != null) return BadRequest("you already like this user.");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };
            sourceUser.LikedUsers.Add(userLike);
            if(await _userRepository.SaveAllAsync()) {
                return Ok();
            }
            return BadRequest("Failed to like user");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes(string predicate) {
            var users = await GetUserLikes(predicate, int.Parse(User.GetUserId()));
            return Ok(users);
        }
        private async Task<IEnumerable<LikeDto>> GetUserLikes(string predicate, int userId)
        {
            var userQuery = _userRepository.GetUserLikesQuery(predicate, userId);
            var likeDto = await userQuery.Select(u => new LikeDto
            {
                UserName = u.UserName,
                KnownAs = u.KnownAs,
                Age = u.DateOfBirth.CalculateAge(),
                PhotoUrl = u.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = u.City,
                Id = u.Id
            }).ToListAsync();
            return likeDto;
        }
    }
}