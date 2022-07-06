using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Data.Repository.Contract
{
    public interface IRepository<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetAsync(string id);

        Task<T> Find(Expression<Func<T, bool>> expression);

        Task<IEnumerable<T>> FindAllWhere(Expression<Func<T, bool>> expression);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void Update(T entity);
        void UpdateRange(IEnumerable<T> entity);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        void Save();
    }
}
