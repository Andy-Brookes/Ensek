using Ensek.Test.Application.Models;

namespace Ensek.Test.Application.Services.Interfaces
{
    public interface IMeterReadingValidator
    {
        Task<ValidateReadingResponse> ValidateReading(MeterReadingModel meterReadingModel);
    }
}
