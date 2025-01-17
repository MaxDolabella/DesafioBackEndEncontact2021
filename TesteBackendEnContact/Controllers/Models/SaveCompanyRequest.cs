﻿using System.ComponentModel.DataAnnotations;
using TesteBackendEnContact.Core.Entities;
using TesteBackendEnContact.Core.Interfaces.Entities;

namespace TesteBackendEnContact.Controllers.Models
{
    public class SaveCompanyRequest : ICompany
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue,ErrorMessage = "{0} is required.")]
        public int ContactBookId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public ICompany ToCompany() => new Company(Id, ContactBookId, Name);
    }
}