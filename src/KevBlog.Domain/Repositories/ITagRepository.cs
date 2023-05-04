using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface ITagRepository
    {
        Task Insert(string tagName, string tagDesc = "", string tagSlug = "");
        Task Delete(int id);
        Task Update(int id, string tagName, string tagDesc, string tagSlug);
        Task<IEnumerable<Tag>> GetAll();
        Task<IEnumerable<Tag>> FindbyTagName(string tagName);
    }
}
