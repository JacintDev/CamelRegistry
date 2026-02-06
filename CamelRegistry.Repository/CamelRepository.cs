using CamelRegistry.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CamelRegistry.Repository
{
    public class CamelRepository : ICamelRepository
    {
        private readonly CamelDbContext _context;

        public CamelRepository(CamelDbContext context)
        {
            _context = context;
        }

        public async Task<Camel> AddAsync(Camel entity)
        {
            await _context.Camels.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) {
            _context.Remove(entity);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Camel>> GetAllAsync(params Expression<Func<Camel, object>>[] includes)
        {
            IQueryable<Camel> query = _context.Camels;
            if (includes != null) {
                foreach (var include in includes)
                {
                    query.Include(include);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Camel?> GetByIdAsync(Guid id)
        {
            return await _context.Camels.FindAsync(id);
        }

        public async Task<Camel> UpdateAsync(Camel entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
