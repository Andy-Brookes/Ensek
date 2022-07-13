using Dapper;
using Ensek.Test.DataAccess.Factories.Interfaces;
using Ensek.Test.Domain.Entities;
using Ensek.Test.Domain.Interfaces.Repositories;

namespace Ensek.Test.DataAccess.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public AccountRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Account> GetById(int accountId)
        {
            using var connection = _connectionFactory.Create();
            var sql = "SELECT Id, FirstName, LastName FROM dbo.Account WHERE Id = @accountId;";
            return await connection.QuerySingleOrDefaultAsync<Account>(sql, new { accountId });
        }
    }
}
