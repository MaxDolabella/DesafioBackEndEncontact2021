using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Repositories.Common;

namespace TesteBackendEnContact.Core.Interfaces.Repositories
{
    public interface ICompanyRepository
        : IRepositoryBase<ICompany>
    {
        Task<IEnumerable<ICompany>> GetByNamesAsync(IEnumerable<string> names);
    }
}