using CamelRegistry.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CamelRegistry.Repository
{
    public interface ICamelRepository
    {
        Task<List<Camel>> GetAllAsync(params Expression<Func<Camel, object>>[] includes);
        Task<Camel?> GetByIdAsync(Guid id);
        Task<Camel> AddAsync(Camel entity);
        Task DeleteAsync(Guid id);
        Task<Camel> UpdateAsync(Camel entity);
    }
}
