using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Repositories;
using TesteBackendEnContact.DataAccess.Database;
using TesteBackendEnContact.DataAccess.Repositories.Common;
using TesteBackendEnContact.Repository.DAO;

namespace TesteBackendEnContact.DataAccess.Repositories
{
    public class ContactBookRepository
        : RepositoryBase<IContactBook>, IContactBookRepository
    {
        public ContactBookRepository(DatabaseConfig databaseConfig)
            : base(databaseConfig)
        { }

        public override async Task<IContactBook> SaveAsync(IContactBook contactBook)
        {
            using var connection = GetConnection();
            var dao = new ContactBookDao(contactBook);

            if (dao.Id == 0)
                dao.Id = await connection.InsertAsync(dao);
            else
                await connection.UpdateAsync(dao);

            return dao.Export();
        }

        public override async Task DeleteAsync(int id)
        {
            // WARNING teste:
            // Vou excluir todos os CONTATOS e COMPANHIAS que fazem parte da AGENDA.
            // TODO delegar essa responsabilidade para a classe de serviço:
            //      - somente excluir quando não houver contatos ou empresas
            //      referenciando essa agenda
            var query = @"
                DELETE FROM Contact WHERE ContactBookId = @id;
                DELETE FROM Company WHERE ContactBookId = @id;
                DELETE FROM ContactBook WHERE Id = @id;";

            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            var rowsAffected = await connection.ExecuteAsync(query, new { id }, transaction);

            if (rowsAffected > 0)
                await transaction.CommitAsync();
            else
                await transaction.RollbackAsync();
        }

        public override async Task<IEnumerable<IContactBook>> GetAllAsync()
        {
            var query = "SELECT * FROM ContactBook";

            using var connection = GetConnection();

            var result = await connection.QueryAsync<ContactBookDao>(query);

            return result?.Select(item => item.Export());
        }

        public override async Task<IContactBook> GetAsync(int id)
        {
            var query = "SELECT * FROM ContactBook WHERE Id = @id";

            using var connection = GetConnection();

            var result = await connection.QuerySingleOrDefaultAsync<ContactBookDao>(query, new { id });

            return result?.Export();
        }

        public async Task<IEnumerable<IContactBook>> GetByNamesAsync(IEnumerable<string> names)
        {
            var query = "SELECT * FROM ContactBook WHERE Name IN @names";

            using var connection = GetConnection();

            var result = await connection.QueryAsync<ContactBookDao>(query, new { names });

            return result?.Select(item => item.Export());
        }
    }
}