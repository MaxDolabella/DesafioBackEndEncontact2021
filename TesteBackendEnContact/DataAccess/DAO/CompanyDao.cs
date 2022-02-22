using Dapper.Contrib.Extensions;
using TesteBackendEnContact.Core.Entities;
using TesteBackendEnContact.Core.Interfaces.Entities;

namespace TesteBackendEnContact.DataAccess.DAO
{
    [Table("Company")]
    public class CompanyDao : ICompany
    {
        [Key] public int Id { get; set; }

        public int ContactBookId { get; set; }
        public string Name { get; set; }

        public CompanyDao()
        { }

        public CompanyDao(ICompany company)
        {
            Id = company.Id;
            ContactBookId = company.ContactBookId;
            Name = company.Name;
        }

        public ICompany Export()
            => new Company(Id, ContactBookId, Name);
    }
}