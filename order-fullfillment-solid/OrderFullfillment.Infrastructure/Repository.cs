using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderFullfillment.Entity.SeedWorks;
using OrderFullfillment.Infrastructure.SeedWorks;

namespace OrderFullfillment.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        private DbSet<T> _dbSet;

        private DbSet<T> DbSet => _dbSet ??= _dbContext.Set<T>();

        public Repository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public async Task<T> GetAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(_ => (_ as EntityBase).Id == id);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }
}