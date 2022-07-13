using Ensek.Test.Application.Models;

namespace Ensek.Test.Application.Services.Interfaces
{
    public interface ICsvFileParser
    {
        IEnumerable<MeterReadingModel> ParseFile(Stream fileStream);
    }
}
