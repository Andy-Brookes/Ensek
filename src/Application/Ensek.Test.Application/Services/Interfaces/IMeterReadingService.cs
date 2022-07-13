using Ensek.Test.Application.Models;

namespace Ensek.Test.Application.Services.Interfaces
{
    public interface IMeterReadingService
    {
        Task<UploadReadingsResponse> UploadReadings(Stream fileStream);
    }
}
