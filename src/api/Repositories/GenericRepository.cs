using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Migrations;

namespace Api
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void UpdateRange(IEnumerable<T> entities);
        Task SaveAsync();
    }

    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal readonly InsuranceContext _context;
        internal readonly DbSet<T> _dbSet;
        protected GenericRepository(InsuranceContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).FirstOrDefaultAsync();;
        }
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;
            return await _dbSet.Where(filter).ToListAsync();
        }
        
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        
        public void UpdateRange(IEnumerable<T> entities)
        {
             _dbSet.UpdateRange(entities);
             _context.SaveChanges();
        }
        
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
