using System.Linq.Expressions;
namespace HBlog.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        Task<TEntity> GetById(int id);
        IQueryable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAllSoftDeleted();
        void Remove(int id);
        Task<int> SaveChangesAsync();
    }
}
