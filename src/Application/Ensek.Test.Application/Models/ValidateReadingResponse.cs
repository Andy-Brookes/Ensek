using Ensek.Test.Domain.Entities;

namespace Ensek.Test.Application.Models
{
    public class ValidateReadingResponse
    {
        public bool IsValid { get; set; }

        public MeterReading? MeterReading { get; set; }

        public static ValidateReadingResponse CreateInvalidResponse()
        {
            return new ValidateReadingResponse();
        }

        public static ValidateReadingResponse CreateValidResponse(MeterReading meterReading)
        {
            return new ValidateReadingResponse
            {
                IsValid = true,
                MeterReading = meterReading
            };
        }
    }
}
