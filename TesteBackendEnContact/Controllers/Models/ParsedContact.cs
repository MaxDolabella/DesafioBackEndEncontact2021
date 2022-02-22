namespace TesteBackendEnContact.Controllers.Models
{
    public record ParsedContact(
       string Name,
       string Phone,
       string Email,
       string Address,
       string CompanyName,
       string ContactBookName);
}