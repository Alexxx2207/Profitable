using Profitable.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Repository.Contract
{
    public interface IRepository<T>
        where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(int skip, int take);

        Task<T> GetAsync(string id);

        Task<T> Find(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> FindAllWhere(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> FindAllWhere(Expression<Func<T, bool>> expression, int skip, int take);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        void Save();
    }
}
