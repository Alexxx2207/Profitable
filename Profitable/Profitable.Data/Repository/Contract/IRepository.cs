using Profitable.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Repository.Contract
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAllAsNoTracking();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void HardDelete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
