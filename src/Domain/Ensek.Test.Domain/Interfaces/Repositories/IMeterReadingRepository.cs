using Ensek.Test.Domain.Entities;

namespace Ensek.Test.Domain.Interfaces.Repositories
{
    public interface IMeterReadingRepository
    {
        Task<int> Create(MeterReading meterReading);

        Task<MeterReading> GetExistingReading(MeterReading meterReading);
    }
}