using AutoMapper;
using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Entities;
using HBlog.Domain.Params;
using HBlog.Domain.Repositories;

namespace HBlog.Application.Services
{
    public class UserService : BaseService ,IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IMapper mapper, IUserRepository userRepository) : base(mapper)
        {
            this._userRepository = userRepository;
        }   

        public async Task<PageList<UserDto>> GetMembersAsync(UserParams userParams)
        {
            User user = await _userRepository.GetUserByUsernameAsync(userParams.CurrentUsername);
            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male";

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

            var members = _mapper.Map<IEnumerable<UserDto>>(usersList);
            return PageList<UserDto>.CreateAsync(members, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<ServiceResult<UserDto>> GetMembersByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is null)
                return ServiceResult.Fail<UserDto>(msg: "Failed to get user");

            UserDto userDto = _mapper.Map<UserDto>(user);
            return ServiceResult.Success(userDto);
        }
        public async Task<ServiceResult<UserDto>> GetMemberByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return ServiceResult.Fail<UserDto>(msg: "Email is required");

            User user = await _userRepository.GetUserByEmailAsync(email);
            if (user is null || user.IsDeleted)
                return ServiceResult.Fail<UserDto>(msg: "User not found");

            return ServiceResult.Success(_mapper.Map<UserDto>(user));
        }

        public async Task<ServiceResult> DeleteMemberAsync(Guid id)
        {
            User user = await _userRepository.GetUserByIdAsync(id);
            if (user is null)
                return ServiceResult.Success(msg: "User not found (already deleted)");

            await _userRepository.SoftDeleteAsync(user);
            return ServiceResult.Success(msg: "User deleted");
        }

        public async Task<bool> UpdateMemberAsync(UserUpdateDto user)
        {
            UserUpdateDto userUpdateDto = new UserUpdateDto();
            _mapper.Map(userUpdateDto, user);
            if (await _userRepository.SaveAllAsync()) return true;

            return false;

        }
    }
}
