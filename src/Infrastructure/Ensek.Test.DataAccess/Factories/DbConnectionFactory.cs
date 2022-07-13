using Ensek.Test.DataAccess.Factories.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Ensek.Test.DataAccess.Factories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Create()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
