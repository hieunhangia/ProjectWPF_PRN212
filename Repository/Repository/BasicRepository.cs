using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repository
{
    public class BasicRepository<TEntity, TKey> where TEntity : class
    {
        public List<TEntity> GetAll()
        {
            using var context = new DbContext();
            return [.. context.Set<TEntity>()];
        }

        public TEntity? GetById(TKey id)
        {
            using var context = new DbContext();
            return context.Set<TEntity>().Find(id);
        }

        public List<TEntity> GetByCondition(Func<TEntity, bool> condition)
        {
            using var context = new DbContext();
            return [.. context.Set<TEntity>().Where(condition)];
        }

        public void Add(TEntity entity)
        {
            using var context = new DbContext();
            context.Add(entity);
            context.SaveChanges();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            using var context = new DbContext();
            context.AddRange(entities);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            using var context = new DbContext();
            context.Update(entity);
            context.SaveChanges();
        }

        public void Delete(TKey id)
        {
            using var context = new DbContext();
            var entity = context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                context.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
