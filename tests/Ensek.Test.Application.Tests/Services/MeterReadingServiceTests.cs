using AutoFixture;
using Ensek.Test.Application.Models;
using Ensek.Test.Application.Services;
using Ensek.Test.Application.Services.Interfaces;
using Ensek.Test.Domain.Entities;
using Ensek.Test.Domain.Interfaces.Repositories;
using Moq;
using Shouldly;

namespace Ensek.Test.Application.Tests.Services
{
    public class MeterReadingServiceTests
    {
        private readonly Mock<ICsvFileParser> _mockCsvFileParser;
        private readonly Mock<IMeterReadingRepository> _mockMeterReadingRepository;
        private readonly Mock<IMeterReadingValidator> _mockMeterReadingValidator;
        private static readonly Fixture _fixture = new();

        public MeterReadingServiceTests()
        {
            _mockCsvFileParser = new Mock<ICsvFileParser>();
            _mockMeterReadingRepository = new Mock<IMeterReadingRepository>();
            _mockMeterReadingValidator = new Mock<IMeterReadingValidator>();
        }

        [Theory]
        [InlineData(true, 5, 5, 0)]
        [InlineData(false, 10, 0, 10)]
        public async Task UploadReadings_returns_expected_values(bool validateReadingSucceeds, int numberOfModels, int expectedSuccessfulReadings, int expectedFailedReadings)
        {
            var meterReadingModels = Enumerable.Repeat(_fixture.Create<MeterReadingModel>(), numberOfModels);
            _mockCsvFileParser
                .Setup(parser => parser.ParseFile(It.IsAny<Stream>()))
                .Returns(meterReadingModels);
            var meterReading = validateReadingSucceeds ? _fixture.Create<MeterReading>() : default;
            var validateReadingResponse = validateReadingSucceeds ? ValidateReadingResponse.CreateValidResponse(meterReading) : ValidateReadingResponse.CreateInvalidResponse();
            _mockMeterReadingValidator
                .Setup(validator => validator.ValidateReading(It.IsAny<MeterReadingModel>()))
                .ReturnsAsync(validateReadingResponse);
            _mockMeterReadingRepository.Setup(repo => repo.Create(It.IsAny<MeterReading>()));
            var fileStream = new MemoryStream();
            var sut = CreateSut();

            var uploadReadingsResponse = await sut.UploadReadings(fileStream);

            uploadReadingsResponse.SuccessfulReadings.ShouldBe(expectedSuccessfulReadings);
            uploadReadingsResponse.FailedReadings.ShouldBe(expectedFailedReadings);
            _mockCsvFileParser.Verify(parser => parser.ParseFile(fileStream), Times.Once);
            _mockMeterReadingValidator.Verify(validator => validator.ValidateReading(It.IsAny<MeterReadingModel>()), Times.Exactly(numberOfModels));
            _mockMeterReadingRepository.Verify(repo => repo.Create(It.IsAny<MeterReading>()), Times.Exactly(validateReadingSucceeds ? numberOfModels : 0));
        }

        private MeterReadingService CreateSut()
        {
            return new MeterReadingService(_mockCsvFileParser.Object, _mockMeterReadingRepository.Object, _mockMeterReadingValidator.Object);
        }
    }
}