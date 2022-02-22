using TesteBackendEnContact.Core.Interfaces.Entities;

namespace TesteBackendEnContact.Core.Entities
{
    public class ContactBook : IContactBook
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public ContactBook(int id, string name)
        {
            Id = id;

            Name = name;
        }
    }
}