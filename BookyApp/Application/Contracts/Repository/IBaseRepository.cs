using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate);
        Task UpdateAsync(T entity);
        Task<T> FindById(object id);

        DbSet<T> GetDb();



         int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
