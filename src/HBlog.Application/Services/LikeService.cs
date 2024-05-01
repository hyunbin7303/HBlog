using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Common.Extensions;
using HBlog.Domain.Entities;
using HBlog.Domain.Params;
using HBlog.Domain.Repositories;

namespace HBlog.Application.Services
{
    public class LikeService : BaseService, ILikeService
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;
        public LikeService(IMapper mapper, ILikesRepository likesRepository, IUserRepository userRepository) : base(mapper)
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
