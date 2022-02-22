using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Repositories.Common;

namespace TesteBackendEnContact.Core.Interfaces.Repositories
{
    public interface IContactRepository
        : IRepositoryBase<IContact>
    {
        Task<IEnumerable<IContact>> SaveMultipleAsync(IEnumerable<IContact> contacts);

        Task<IEnumerable<IContact>> GetByTermAsync(string searchTerm);

        Task<IEnumerable<IContact>> GetByCompanyAsync(int companyId);
    }
}