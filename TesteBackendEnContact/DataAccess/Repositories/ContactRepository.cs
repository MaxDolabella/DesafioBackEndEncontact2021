using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Entities;
using TesteBackendEnContact.Core.Interfaces.Repositories;
using TesteBackendEnContact.DataAccess.DAO;
using TesteBackendEnContact.DataAccess.Database;
using TesteBackendEnContact.DataAccess.Repositories.Common;

namespace TesteBackendEnContact.DataAccess.Repositories
{
    public class ContactRepository
        : RepositoryBase<IContact>, IContactRepository
    {
        public ContactRepository(DatabaseConfig databaseConfig)
            : base(databaseConfig)
        { }

        public override async Task<IContact> SaveAsync(IContact contact)
        {
            using var connection = GetConnection();
            var dao = new ContactDao(contact);

            if (dao.Id == 0)
                dao.Id = await connection.InsertAsync(dao);
            else
                await connection.UpdateAsync(dao);

            return dao.Export();
        }

        public async Task<IEnumerable<IContact>> SaveMultipleAsync(IEnumerable<IContact> entities)
        {
            var query_to_get_last_id = "SELECT seq FROM SQLITE_SEQUENCE WHERE name='Contact';";

            using var connection = GetConnection();

            var daos = entities.Select(contact => new ContactDao(contact)).ToList();

            await connection.InsertAsync(daos);
            var identity = await connection.QuerySingleAsync<long>(query_to_get_last_id);

            for (int i = 0; i < daos.Count; i++)
                daos[i].Id = (int)identity - daos.Count + i + 1; ;

            return daos.Select(dao => dao.Export());
        }

        public override async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM Contact WHERE Id = @id;";

            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            var rowsAffected = await connection.ExecuteAsync(query, new { id }, transaction);

            if (rowsAffected == 1)
                await transaction.CommitAsync();
            else
                await transaction.RollbackAsync();
        }

        public override async Task<IEnumerable<IContact>> GetAllAsync()
        {
            var query = "SELECT * FROM Contact";

            using var connection = GetConnection();

            var result = await connection.QueryAsync<ContactDao>(query);

            return result?.Select(item => item.Export());
        }

        public override async Task<IContact> GetAsync(int id)
        {
            var query = "SELECT * FROM Contact WHERE Id = @id";

            using var connection = GetConnection();

            var result = await connection.QuerySingleOrDefaultAsync<ContactDao>(query, new { id });

            return result?.Export();
        }

        public async Task<IEnumerable<IContact>> GetByCompanyAsync(int companyId)
        {
            var query = "SELECT * FROM Contact WHERE CompanyId=@companyId";

            using var connection = GetConnection();
            var result = await connection.QueryAsync<ContactDao>(query, new { companyId });

            return result?.Select(item => item.Export());
        }

        public async Task<IEnumerable<IContact>> GetByTermAsync(string searchTerm)
        {
            searchTerm = $"%{searchTerm}%";

            var query = @"
                SELECT Contact.* FROM Contact
                LEFT JOIN Company
                    ON Company.Id=Contact.CompanyId
                WHERE Company.Name LIKE @searchTerm
                	OR Contact.Name LIKE @searchTerm
                    OR Contact.Phone LIKE @searchTerm
                    OR Contact.Email LIKE @searchTerm
                    OR Contact.Address LIKE @searchTerm;";

            using var connection = GetConnection();
            var result = await connection.QueryAsync<ContactDao>(query, new { searchTerm });

            return result?.Select(item => item.Export());
        }
    }
}