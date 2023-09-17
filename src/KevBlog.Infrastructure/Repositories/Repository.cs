using KevBlog.Domain.Common;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KevBlog.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext _dataContext;
        protected readonly DbSet<TEntity> _dbSet;
        public Repository(DataContext context)
        {
            _dataContext = context;
            _dbSet = _dataContext.Set<TEntity>();
        }
        public virtual void Add(TEntity obj) => _dbSet.Add(obj);
        public virtual IQueryable<TEntity> GetAll() => _dbSet;
        public virtual IQueryable<TEntity> GetAllSoftDeleted()
            => _dbSet.IgnoreQueryFilters().Where(x => EF.Property<bool>(x, "IsDeleted"));
        public virtual TEntity GetById(TEntity id) => _dbSet.Find(id);
        public virtual void Remove(TEntity id)
            => _dbSet.Remove(_dbSet.Find(id));
        public virtual int SaveChanges()
            => _dataContext.SaveChanges();
    }
}
