using System.Collections.Generic;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Core.Interfaces.Repositories.Common
{
    public interface IRepositoryBase<TEntity>
        where TEntity : class
    {
        Task<TEntity> SaveAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetAsync(int id);
    }
}