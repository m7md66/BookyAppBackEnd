using Application.Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Persistence.Repository
{
    public class BaseRepository<T>: IBaseRepository<T> where T : class
    {
        private  DbSet<T> _dbSet;
        private readonly AppDbContext _context;
       
        public BaseRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet = _context.Set<T>();
        }


        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public async Task AddRangeAsync(T[] entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public void Delete(T entity) {

            _dbSet.Remove(entity);
        }


        //public IEnumerable<T> GetAll() { 
        //return _dbSet.OrderByDescending()
        //}

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).FirstOrDefaultAsync();
        }
        public async Task<T> FindById(object id)
        {
            return await  _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsNoTracking().AsQueryable();
        }

        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }




















        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
