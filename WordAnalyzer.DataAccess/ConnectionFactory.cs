using Npgsql;
using System;
using System.Data;
using System.Data.Common;

namespace WordAnalyzer.DataAccess
{
    public static class ConnectionFactory
    {
        public static IDbConnection Create(string connectionString, Provider provider)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException();
            }

            DbConnection connection = provider switch
            {
                Provider.PostgreSQL => new NpgsqlConnection(),
                _ => throw new NotImplementedException(),
            };

            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }
    }
}
