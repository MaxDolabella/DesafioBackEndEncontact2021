using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interfaces.Repositories.Common;
using TesteBackendEnContact.DataAccess.Database;

namespace TesteBackendEnContact.DataAccess.Repositories.Common
{
    public abstract class RepositoryBase<TEntity>
        : IRepositoryBase<TEntity>
        where TEntity : class
    {
        protected readonly DatabaseConfig databaseConfig;

        protected DbConnection GetConnection()
            => new SqliteConnection(databaseConfig.ConnectionString);

        protected RepositoryBase(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public abstract Task<TEntity> SaveAsync(TEntity entity);

        public abstract Task DeleteAsync(int id);

        public abstract Task<IEnumerable<TEntity>> GetAllAsync();

        public abstract Task<TEntity> GetAsync(int id);
    }
}