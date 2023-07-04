using KevBlog.Domain.Entities;

namespace KevBlog.Domain.Repositories
{
    public interface ITagRepository
    {
        Task Insert(Tag tag);
        Task Delete(int id);
        Task Update(Tag tag);
        Task<IEnumerable<Tag>> GetAll();
        Task<Tag> GetById(int id);  
        Task<IEnumerable<Tag>> FindbyTagName(string tagName);
    }
}
