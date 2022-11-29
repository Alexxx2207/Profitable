namespace Profitable.Data.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Profitable.Data.Repository.Contract;
    using Profitable.Models.Contracts;

    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDeletebleEntity
    {
        private readonly ApplicationDbContext dbContext;

        private readonly DbSet<TEntity> table;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.table = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return this.table;
        }

        public IQueryable<TEntity> GetAllAsNoTracking()
        {
            return this.table.AsNoTracking();
        }

        public async Task AddAsync(TEntity entity)
        {
            await this.table.AddAsync(entity).AsTask();
        }

        public void Update(TEntity entity)
        {
            var entry = this.dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.table.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void HardDelete(TEntity entity)
        {
            this.table.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return this.dbContext.SaveChangesAsync();
        }
    }
}
