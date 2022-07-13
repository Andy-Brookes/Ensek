using Ensek.Test.Application.Services;
using Shouldly;
using System.Reflection;

namespace Ensek.Test.Application.Tests.Services
{
    public class CsvFileParserTests
    {
        [Fact]
        public void ParseFile_creates_expected_number_of_MeterReadingModel_instances()
        {
            const int HeaderRow = 1;
            var fileLocation = $"{Assembly.GetExecutingAssembly().Location.Split("\\bin").First()}\\TestData";
            var filename = Path.Combine(fileLocation, "test.csv");
            var expectedNumberOfMeterReadingModels = File.ReadLines(filename).Count() - HeaderRow;
            using var fileStream = File.OpenRead(filename);
            var sut = CreateSut();
            
            var meterReadingModels = sut.ParseFile(fileStream);

            meterReadingModels.Count().ShouldBe(expectedNumberOfMeterReadingModels);
        }

        private static CsvFileParser CreateSut()
        {
            return new CsvFileParser();
        }
    }
}