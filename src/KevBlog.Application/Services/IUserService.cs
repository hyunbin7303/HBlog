using KevBlog.Application.Common;
using KevBlog.Application.DTOs;
using KevBlog.Domain.Common;
using KevBlog.Domain.Params;

namespace KevBlog.Application.Services
{
    public interface ITagService
    {
        Task<ServiceResult> AddTagToPost(int postId, string tagName);
    }
    public interface IUserService
    {
        Task<PageList<MemberDto>> GetMembersAsync(UserParams userParams);
        Task<bool> UpdateMemberAsync(MemberUpdateDto User);
        Task<ServiceResult<MemberDto>> GetMembersByUsernameAsync(string username);

    }
}
