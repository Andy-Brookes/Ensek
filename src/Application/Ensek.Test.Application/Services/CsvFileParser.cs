using CsvHelper;
using CsvHelper.Configuration;
using Ensek.Test.Application.Mapping;
using Ensek.Test.Application.Models;
using Ensek.Test.Application.Services.Interfaces;
using System.Globalization;

namespace Ensek.Test.Application.Services
{
    public class CsvFileParser : ICsvFileParser
    {
        public IEnumerable<MeterReadingModel> ParseFile(Stream fileStream)
        {
            using var streamReader = new StreamReader(fileStream);
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };

            using var csv = new CsvReader(streamReader, configuration);
            csv.Context.RegisterClassMap<MeterReadingMap>();

            return csv.GetRecords<MeterReadingModel>().ToList();
        }
    }
}
