namespace TesteBackendEnContact.Core.Interfaces.Entities
{
    public interface ICompany
    {
        int Id { get; }
        int ContactBookId { get; }
        string Name { get; }
    }
}