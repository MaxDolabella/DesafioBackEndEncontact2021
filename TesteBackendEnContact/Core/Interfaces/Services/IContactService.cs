using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Entities;

namespace TesteBackendEnContact.Core.Interfaces.Services
{
    public interface IContactService
    {
        Task<IEnumerable<IContact>> GetAllAsync();
        Task<IEnumerable<IContact>> GetByCompanyAsync(int companyId);
        Task<IEnumerable<IContact>> SaveContactsFromCSVFileAsync(Stream stream);
        Task<IEnumerable<IContact>> SearchContacts(string searchTerm);
    }
}