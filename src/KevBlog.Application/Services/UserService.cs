using AutoMapper;
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
            var userQuery = _userRepository.GetUserQuery();
            userQuery = userQuery.Where(x => x.UserName != userParams.CurrentUsername);
            userQuery = userQuery.Where(x => x.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            userQuery = userQuery.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

            userQuery = userParams.OrderBy switch
            {
                "created" => userQuery.OrderByDescending(x => x.Created),
                _ => userQuery.OrderByDescending(x => x.LastActive)
            };

            var memberQuery = _mapper.ProjectTo<MemberDto>(userQuery);
            return await PageList<MemberDto>.CreateAsync(memberQuery, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<MemberDto> GetMembersByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<MemberDto>(user) ?? null;
        }
        public async Task<bool> UpdateMemberAsync(MemberUpdateDto user)
        {
            if(user is null)
                throw new ArgumentNullException(nameof(user));

            MemberUpdateDto memberUpdateDto = new MemberUpdateDto();
            _mapper.Map(memberUpdateDto, user);
            if (await _userRepository.SaveAllAsync()) return true;

            return false;

        }
    }
}
