using System.ComponentModel.DataAnnotations;
using TesteBackendEnContact.Core.Entities;
using TesteBackendEnContact.Core.Interfaces.Entities;

namespace TesteBackendEnContact.Controllers.Models
{
    public class SaveContactBookRequest : IContactBook
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public IContactBook ToContactBook() => new ContactBook(Id, Name);
    }
}