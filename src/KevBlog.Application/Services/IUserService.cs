using KevBlog.Contract.Common;
using KevBlog.Contract.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Params;

namespace KevBlog.Application.Services
{
    public interface IUserService
    {
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<bool> UpdateMemberAsync(MemberUpdateDto User);
        Task<ServiceResult<MemberDto>> GetMembersByUsernameAsync(string username);
    }
}
