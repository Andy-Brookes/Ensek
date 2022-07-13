using Ensek.Test.Application.Models;
using Ensek.Test.Application.Services.Interfaces;
using Ensek.Test.Domain.Interfaces.Repositories;

namespace Ensek.Test.Application.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly ICsvFileParser _csvFileParser;
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly IMeterReadingValidator _meterReadingValidator;

        public MeterReadingService(ICsvFileParser csvFileParser, IMeterReadingRepository meterReadingRepository, IMeterReadingValidator meterReadingValidator)
        {
            _csvFileParser = csvFileParser;
            _meterReadingRepository = meterReadingRepository;
            _meterReadingValidator = meterReadingValidator;
        }

        public async Task<UploadReadingsResponse> UploadReadings(Stream fileStream)
        {
            var failedReadings = 0;
            var successfulReadings = 0;

            foreach (var meterReadingModel in _csvFileParser.ParseFile(fileStream))
            {
                var validateReadingResponse = await _meterReadingValidator.ValidateReading(meterReadingModel);
                if (validateReadingResponse.IsValid)
                {
                    await _meterReadingRepository.Create(validateReadingResponse.MeterReading);
                    successfulReadings++;
                }
                else
                {
                    failedReadings++;
                }                
            }

            await fileStream.DisposeAsync();

            return UploadReadingsResponse.Create(failedReadings, successfulReadings);
        }
    }
}
