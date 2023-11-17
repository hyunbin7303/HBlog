using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task Delete(Tag tag);
        Task Update(Tag tag);
        Task<IEnumerable<Tag>> GetAll();
        IEnumerable<Tag> FindbyTagName(string tagName);
    }
}
