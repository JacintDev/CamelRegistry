using CamelRegistry.Entities;
using System.Linq.Expressions;

namespace CamelRegistry.Logic
{
    public interface ICamelLogic
    {
        Task<Camel> AddAsync(CamelCreateModel createModel);
        Task DeleteAsync(Guid id);
        Task<List<Camel>> GetAllAsync(params Expression<Func<Camel, object>>[] includes);
        Task<Camel> GetByIdAsync(Guid id);
        Task<Camel> UpdateAsync(Guid id, CamelUpdateModel updateModel);
    }
}