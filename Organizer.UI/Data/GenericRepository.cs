using System.Data.Entity;
using System.Threading.Tasks;

namespace Organizer.UI.Data
{
    public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity> where TDbContext : DbContext
        where TEntity : class
    {
        protected readonly TDbContext _dbContext;

        protected GenericRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Add(TEntity model)
        {
            return _dbContext.Set<TEntity>().Add(model);
        }

        public void Delete(TEntity model)
        {
            _dbContext.Set<TEntity>().Remove(model);
            _dbContext.SaveChanges();
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            var friend = await _dbContext.Set<TEntity>().FindAsync(id);
            return friend;
        }

        public bool HasChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
