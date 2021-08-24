using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;

namespace WordAnalyzer.DataAccess
{
    public class DapperContext : IDapperContext
    {
        private readonly string _connectionString;
        private readonly Provider _provider;

        public DapperContext(IDapperSettings settings)
        {
            _connectionString = settings.ConnectionString;
            _provider = settings.Provider;
        }

        public async Task Command(IQueryObject queryObject)
        {
            await Execute(query => query.ExecuteAsync(queryObject.Sql, queryObject.Params)).ConfigureAwait(false);
        }

        private async Task<T> Execute<T>(Func<IDbConnection, Task<T>> query)
        {
            using (var connection = ConnectionFactory.Create(_connectionString, _provider))
            {
                T result = await query(connection).ConfigureAwait(false);
                connection.Close();
                return result;
            }
        }
    }
}