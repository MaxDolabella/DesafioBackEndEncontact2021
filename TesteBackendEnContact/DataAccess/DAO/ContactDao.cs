using Dapper.Contrib.Extensions;
using TesteBackendEnContact.Core.Entities;
using TesteBackendEnContact.Core.Interfaces.Entities;

namespace TesteBackendEnContact.DataAccess.DAO
{
    [Table("Contact")]
    public class ContactDao : IContact
    {
        [Key] public int Id { get; set; }

        public int ContactBookId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public ContactDao()
        { }

        public ContactDao(IContact contact)
        {
            Id = contact.Id;
            ContactBookId = contact.ContactBookId;
            CompanyId = contact.CompanyId;

            Name = contact.Name;
            Phone = contact.Phone;
            Email = contact.Email;
            Address = contact.Address;
        }

        public IContact Export()
            => new Contact(Id, ContactBookId, CompanyId, Name, Phone, Email, Address);
    }
}