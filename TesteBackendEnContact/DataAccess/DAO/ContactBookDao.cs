using Dapper.Contrib.Extensions;
using TesteBackendEnContact.Core.Entities;
using TesteBackendEnContact.Core.Interfaces.Entities;

namespace TesteBackendEnContact.Repository.DAO
{
    [Table("ContactBook")]
    public class ContactBookDao : IContactBook
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }

        public ContactBookDao()
        { }

        public ContactBookDao(IContactBook contactBook)
        {
            Id = contactBook.Id;
            Name = contactBook.Name;
        }

        public IContactBook Export()
            => new ContactBook(Id, Name);
    }
}