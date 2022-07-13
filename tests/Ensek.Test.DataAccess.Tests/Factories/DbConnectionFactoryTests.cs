using Ensek.Test.DataAccess.Factories;
using Shouldly;
using System.Data.SqlClient;

namespace Ensek.Test.DataAccess.Tests.Factories
{
    public class DbConnectionFactoryTests
    {
        [Fact]
        public void Create_returns_SqlConnection_instance()
        {
            var connectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=Ensek.Test.Database;Integrated Security=True;Pooling=False;Connect Timeout=30"; ;
            var sut = CreateSut(connectionString);

            var dbConnection = sut.Create();

            dbConnection.ShouldBeOfType<SqlConnection>();
            dbConnection.ConnectionString.ShouldBe(connectionString);
        }

        private DbConnectionFactory CreateSut(string connectionString)
        {
            return new DbConnectionFactory(connectionString);
        }
    }
}