using HBlog.Contract.Common;
using HBlog.Contract.DTOs;
using HBlog.Domain.Common;
using HBlog.Domain.Params;

namespace HBlog.Application.Services
{
    public interface IUserService
    {
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<bool> UpdateMemberAsync(UserUpdateDto User);
        Task<ServiceResult<MemberDto>> GetMembersByUsernameAsync(string username);
    }
}
