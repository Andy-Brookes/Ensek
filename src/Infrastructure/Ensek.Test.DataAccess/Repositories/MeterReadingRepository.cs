using Dapper;
using Ensek.Test.DataAccess.Factories.Interfaces;
using Ensek.Test.Domain.Entities;
using Ensek.Test.Domain.Interfaces.Repositories;

namespace Ensek.Test.DataAccess.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public MeterReadingRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> Create(MeterReading meterReading)
        {
            using var connection = _connectionFactory.Create();
            var sql = @"INSERT INTO dbo.MeterReading
                        (
                            ReadingDateTime,
                            ReadingValue,
                            AccountId
                        )
                        VALUES
                        (
                            @ReadingDateTime,
                            @ReadingValue,
                            @AccountId
                        );
                        SELECT SCOPE_IDENTITY();";
            return await connection.ExecuteScalarAsync<int>(sql, meterReading);
        }

        public async Task<MeterReading> GetExistingReading(MeterReading meterReading)
        {
            using var connection = _connectionFactory.Create();
            var sql = @"SELECT  Id,
                                ReadingDateTime,
                                ReadingValue,
                                AccountId
                        FROM    dbo.MeterReading
                        WHERE   ReadingDateTime = @ReadingDateTime
                        AND     ReadingValue = @ReadingValue
                        AND     AccountId = @AccountId;";
            return await connection.QuerySingleOrDefaultAsync<MeterReading>(sql, meterReading);
        }
    }
}
