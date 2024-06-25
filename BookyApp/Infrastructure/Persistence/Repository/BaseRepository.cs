using Application.Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class BaseRepository<T>: IRepository<T> where T : class
    {
        private  DbSet<T> _dbSet;
        private readonly AppDbContext _appDbContext;
       
        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
        }


        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

    }
}
