using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Repositories.Common;

namespace TesteBackendEnContact.Core.Interfaces.Repositories
{
    public interface IContactBookRepository
        : IRepositoryBase<IContactBook>
    {
        Task<IEnumerable<IContactBook>> GetByNamesAsync(IEnumerable<string> names);
    }
}