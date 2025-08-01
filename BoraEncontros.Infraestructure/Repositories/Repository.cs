using BoraEncontros.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BoraEncontros.Infraestructure.Repositories
{
    internal abstract class Repository(BoraEncontrosDbContext dbContext) : IRepository
    {
        public ICommitScope CommitScope => dbContext;
        public IQueryable<TEntity> Set<TEntity>() where TEntity : Entity
        {
            var query = dbContext.Set<TEntity>();
            return query;
        }
        public async Task<TEntity?> FirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity : Entity
        {
            return await Set<TEntity>().FirstOrDefaultAsync(where);
        }

        public bool Any<TEntity>(Expression<Func<TEntity, bool>>? where = null) where TEntity : Entity
        {
            return where == null ? Set<TEntity>().Any() : Set<TEntity>().Any(where);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : Entity
        {
            dbContext.Remove(entity);
        }

        public void RemoveRange<TEntity>(TEntity entity) where TEntity : Entity
        {
            dbContext.RemoveRange(entity);
        }
    }
}
