namespace KevBlog.Domain.Repositories
{
    public interface ITagRepository
    {
        Task AddTag(string tagName, string tagDesc = "", string tagSlug = "");
        Task Delete(int id);
        Task Update(int id, string tagName, string tagDesc, string tagSlug);
    }
}
