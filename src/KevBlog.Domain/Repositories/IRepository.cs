using KevBlog.Domain.Common;
namespace KevBlog.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        TEntity GetById(TEntity id);
        IQueryable<TEntity> GetAll();
        //IQueryable<TEntity> GetAll(ISpecification<TEntity> spec);
        IQueryable<TEntity> GetAllSoftDeleted();
        void Remove(TEntity id);
        int SaveChanges();
    }
}
