using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Common.Extensions;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Helpers;
using Microsoft.VisualBasic;

namespace KevBlog.Application.Services
{
    public class LikeSerivce : ILikeService
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        public LikeSerivce(ILikesRepository likesRepository, IUserRepository userRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        public async Task<ServiceResult> AddLike(int sourceUserId, string username)
        {
            var likedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);
            if (likedUser is null) return ServiceResult.NotFound(msg: "Cannot find liked User.");
            if (sourceUser.UserName == username) return ServiceResult.Fail(msg: "You cannot like yourself.");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
            if (userLike != null) return ServiceResult.Fail(new List<string> { "BadRequest" } ,"You already like this user.");

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id
            };
            sourceUser.LikedUsers.Add(userLike);
            if (await _userRepository.SaveAllAsync())
                return ServiceResult.Success();

            return ServiceResult.Fail(new List<string> { "BadRequest" }, msg: "Bad Request.");
        }

        public async Task<PageList<LikeDto>> GetUserLikePageList(LikesParams likesParam)
        {
            var userQuery = _userRepository.GetUserLikesQuery(likesParam.Predicate, likesParam.UserId);
            var likeDto = userQuery.Select(u => new LikeDto
            {
                UserName = u.UserName,
                KnownAs = u.KnownAs,
                Age = u.DateOfBirth.CalculateAge(),
                PhotoUrl = u.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = u.City,
                Id = u.Id
            });
            return await PageList<LikeDto>.CreateAsync(likeDto, likesParam.PageNumber, likesParam.PageSize);
        }
    }
}
