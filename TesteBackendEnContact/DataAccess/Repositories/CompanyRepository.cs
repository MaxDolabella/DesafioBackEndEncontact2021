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
    public class CompanyRepository
        : RepositoryBase<ICompany>, ICompanyRepository
    {
        public CompanyRepository(DatabaseConfig databaseConfig)
            : base(databaseConfig)
        { }

        public override async Task<ICompany> SaveAsync(ICompany company)
        {
            using var connection = GetConnection();
            var dao = new CompanyDao(company);

            if (dao.Id == 0)
                dao.Id = await connection.InsertAsync(dao);
            else
                await connection.UpdateAsync(dao);

            return dao.Export();
        }

        public override async Task DeleteAsync(int id)
        {
            var query = @"
                DELETE FROM Company WHERE Id = @id;
                UPDATE Contact SET CompanyId = null WHERE CompanyId = @id;";

            using var connection = GetConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            var rowsAffected = await connection.ExecuteAsync(query, new { id }, transaction);

            if (rowsAffected == 1)
                await transaction.CommitAsync();
            else
                await transaction.RollbackAsync();
        }

        public override async Task<IEnumerable<ICompany>> GetAllAsync()
        {
            var query = "SELECT * FROM Company";
            
            using var connection = GetConnection();

            var result = await connection.QueryAsync<CompanyDao>(query);

            return result?.Select(item => item.Export());
        }

        public override async Task<ICompany> GetAsync(int id)
        {
            var query = "SELECT * FROM Company WHERE Id = @id";
            
            using var connection = GetConnection();

            var result = await connection.QuerySingleOrDefaultAsync<CompanyDao>(query, new { id });

            return result?.Export();
        }

        public async Task<IEnumerable<ICompany>> GetByNamesAsync(IEnumerable<string> names)
        {
            var query = "SELECT * FROM Company WHERE Name IN @names";
            
            using var connection = GetConnection();

            var result = await connection.QueryAsync<CompanyDao>(query, new { names });

            return result?.Select(item => item.Export());
        }
    }
}