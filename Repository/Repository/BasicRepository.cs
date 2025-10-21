using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository.Repository
{
    public class BasicRepository<TEntity, TKey>(IDbContextFactory<DbContext> contextFactory) where TEntity : class
    {
        protected readonly IDbContextFactory<DbContext> _contextFactory = contextFactory;

        public List<TEntity> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<TEntity>()];
        }

        public TEntity? GetById(TKey id)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Set<TEntity>().Find(id);
        }

        public List<TEntity> GetByCondition(Expression<Func<TEntity, bool>> condition)
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<TEntity>().Where(condition)];
        }

        public void Add(TEntity entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Add(entity);
            context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            using var context = _contextFactory.CreateDbContext();
            context.AddRange(entities);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Update(entity);
            context.SaveChanges();
        }

        public void Delete(TKey id)
        {
            using var context = _contextFactory.CreateDbContext();
            var entity = context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                context.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
