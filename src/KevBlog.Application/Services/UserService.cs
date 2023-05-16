using AutoMapper;
using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;

namespace KevBlog.Application.Services
{
    public class UserService : BaseService ,IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IMapper mapper, IUserRepository userRepository) : base(mapper)
        {
            this._userRepository = userRepository;
        }   

        public async Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var usersList = await _userRepository.GetUsersAsync();
            usersList = usersList.Where(x => x.UserName != userParams.CurrentUsername);
            usersList = usersList.Where(x => x.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            usersList = usersList.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            usersList = userParams.OrderBy switch
            {
                "created" => usersList.OrderByDescending(x => x.Created),
                _ => usersList.OrderByDescending(x => x.LastActive)
            };

            var members = _mapper.Map<IEnumerable<MemberDto>>(usersList);
            return PageList<MemberDto>.CreateAsync(members, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<ServiceResult<MemberDto>> GetMembersByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is null)
                return ServiceResult.Fail<MemberDto>(msg: "Failed to get user");

            MemberDto member = _mapper.Map<MemberDto>(user);
            return ServiceResult.Success(member);
        }
        public async Task<bool> UpdateMemberAsync(MemberUpdateDto user)
        {
            MemberUpdateDto memberUpdateDto = new MemberUpdateDto();
            _mapper.Map(memberUpdateDto, user);
            if (await _userRepository.SaveAllAsync()) return true;

            return false;

        }
    }
}
