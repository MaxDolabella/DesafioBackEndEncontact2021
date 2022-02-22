using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Controllers.Models;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Repositories;
using TesteBackendEnContact.Core.Interfaces.Services;
using TesteBackendEnContact.Core.Utils;

namespace TesteBackendEnContact.Core.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IContactBookRepository _contactBookRepository;
        private readonly ICompanyRepository _companyRepository;

        public ContactService(
            IContactRepository contactRepository,
            IContactBookRepository contactBookRepository,
            ICompanyRepository companyRepository)
        {
            _contactRepository = contactRepository;
            _contactBookRepository = contactBookRepository;
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<IContact>> SaveContactsFromCSVFileAsync(Stream stream)
        {
            // Get .csv file contents
            var contents = await GetContentsAsync(stream);

            // Get valid models
            var parsedContacts = ContentsToParsedContacts(contents);

            // Get already registered ContactBooks from csv
            var contactBooks = await GetContactBooksFromParsedContactsAsync(parsedContacts);

            // Get already registered Companies from csv
            var companies = await GetCompaniesFromParsedContactsAsync(parsedContacts);

            // Get models to save
            var saveContactRequests = ParsedContactsToModels(parsedContacts, contactBooks, companies);

            return await _contactRepository.SaveMultipleAsync(saveContactRequests);
        }

        public async Task<IEnumerable<IContact>> GetAllAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

        public async Task<IEnumerable<IContact>> SearchContacts(string searchTerm)
        {
            return await _contactRepository.GetByTermAsync(searchTerm);
        }

        public async Task<IEnumerable<IContact>> GetByCompanyAsync(int companyId)
        {
            return await _contactRepository.GetByCompanyAsync(companyId);
        }

        #region Private Methods

        private static async Task<IEnumerable<string>> GetContentsAsync(Stream stream)
        {
            var contents = new HashSet<string>();

            using (var reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                    contents.Add(await reader.ReadLineAsync());
            }

            return contents;
        }

        private static IEnumerable<ParsedContact> ContentsToParsedContacts(IEnumerable<string> contents)
        {
            var parsedContacts = new HashSet<ParsedContact>();
            var parser = new ContactParser();

            foreach (var line in contents)
                if (parser.TryParse(line, out ParsedContact parsedContact))
                    parsedContacts.Add(parsedContact);

            return parsedContacts;
        }

        private static IEnumerable<SaveContactRequest> ParsedContactsToModels(
            IEnumerable<ParsedContact> parsedContacts,
            IEnumerable<IContactBook> contactBooks,
            IEnumerable<ICompany> companies)
        {
            var saveContactRequests = new HashSet<SaveContactRequest>();

            foreach (var item in parsedContacts)
            {
                var contactBookId = contactBooks
                    .FirstOrDefault(cb => cb.Name.ToLower()
                        == item.ContactBookName.ToLower())?.Id ?? 0;

                if (contactBookId == 0) continue;

                var companyId = companies
                    .FirstOrDefault(c => c.Name.ToLower()
                        == item.CompanyName.ToLower())?.Id ?? 0;

                var saveContactRequest = new SaveContactRequest
                {
                    Id = 0,
                    Name = item.Name,
                    Phone = item.Phone,
                    Address = item.Address,
                    Email = item.Email,

                    CompanyId = companyId,
                    ContactBookId = contactBookId
                };

                saveContactRequests.Add(saveContactRequest);
            }

            return saveContactRequests;
        }

        private async Task<IEnumerable<IContactBook>> GetContactBooksFromParsedContactsAsync(IEnumerable<ParsedContact> parsedContacts)
        {
            var distinctContactBookNames = parsedContacts
                .Select(pc => pc.ContactBookName)
                .Distinct();

            return await _contactBookRepository
                .GetByNamesAsync(distinctContactBookNames);
        }

        private async Task<IEnumerable<ICompany>> GetCompaniesFromParsedContactsAsync(IEnumerable<ParsedContact> parsedContacts)
        {
            var distinctCompanyNames = parsedContacts
                .Select(pc => pc.CompanyName)
                .Distinct();

            return await _companyRepository
                .GetByNamesAsync(distinctCompanyNames);
        }

        #endregion Private Methods
    }
}