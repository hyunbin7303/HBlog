using KevBlog.Domain.Common;
namespace KevBlog.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity obj);
        Task<TEntity> GetById(int id);
        IQueryable<TEntity> GetAll();
        //IQueryable<TEntity> GetAll(ISpecification<TEntity> spec);
        IQueryable<TEntity> GetAllSoftDeleted();
        void Remove(int id);
        Task<int> SaveChangesAsync();
    }
}
