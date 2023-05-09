using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Params;
using KevBlog.Domain.Repositories;

namespace KevBlog.Application.Services
{
    public interface IUserService
    {
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<bool> UpdateMemberAsync(MemberUpdateDto User);
        Task<MemberDto> GetMembersByUsernameAsync(string username);

    }
}
