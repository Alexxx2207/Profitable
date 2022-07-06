using Microsoft.EntityFrameworkCore;
using Profitable.Data.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Repository
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly ApplicationDbContext dbContext;

        private readonly DbSet<T> table;

        public Repository(ApplicationDbContext dbContext, DbSet<T> table)
        {
            this.dbContext = dbContext;
            this.table = table;
        }

        public void Add(T entity)
        {
            table.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            table.AddRange(entities);
        }

        public void Delete(T entity)
        {
            table.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            table.RemoveRange(entities);
        }

        public async Task<T> Find(Expression<Func<T, bool>> expression)
        {
            return await table.FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> FindAllWhere(Expression<Func<T, bool>> expression)
        {
            return await table.Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();

        }

        public async Task<T> GetAsync(string id)
        {
            return await table.FindAsync(id);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            table.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entity)
        {
            table.UpdateRange(entity);
        }
    }
}
