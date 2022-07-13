using AutoFixture;
using AutoMapper;
using Ensek.Test.Application.Models;
using Ensek.Test.Application.Services;
using Ensek.Test.Domain.Entities;
using Ensek.Test.Domain.Interfaces.Repositories;
using Moq;
using Shouldly;

namespace Ensek.Test.Application.Tests.Services
{
    public class MeterReadingValidatorTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IMeterReadingRepository> _mockMeterReadingRepository;
        private readonly Mock<IMapper> _mockMapper;
        private static readonly Fixture _fixture = new();

        public MeterReadingValidatorTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMeterReadingRepository = new Mock<IMeterReadingRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ValidateReading_returns_invalid_response_when_account_does_not_exist()
        {
            Account account = null;
            _mockAccountRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(account);
            var meterReadingModel = _fixture.Create<MeterReadingModel>();
            var sut = CreateSut();

            var validateReadingResponse = await sut.ValidateReading(meterReadingModel);

            validateReadingResponse.IsValid.ShouldBeFalse();
            validateReadingResponse.MeterReading.ShouldBeNull();
            _mockAccountRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task TryParse_returns_invalid_response_when_reading_value_is_empty(string readingValue)
        {
            var account = _fixture.Create<Account>();
            _mockAccountRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(account);
            var meterReadingModel = _fixture.Create<MeterReadingModel>();
            meterReadingModel.ReadingValue = readingValue;
            var sut = CreateSut();

            var validateReadingResponse = await sut.ValidateReading(meterReadingModel);

            validateReadingResponse.IsValid.ShouldBeFalse();
            validateReadingResponse.MeterReading.ShouldBeNull();
            _mockAccountRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData("VOID")]
        [InlineData("123XY")]
        [InlineData("-12345")]
        public async Task ValidateReading_returns_invalid_response_when_reading_value_is_not_a_positive_integer(string readingValue)
        {
            var account = _fixture.Create<Account>();
            _mockAccountRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(account);
            var meterReadingModel = _fixture.Create<MeterReadingModel>();
            meterReadingModel.ReadingValue = readingValue;
            var sut = CreateSut();

            var validateReadingResponse = await sut.ValidateReading(meterReadingModel);

            validateReadingResponse.IsValid.ShouldBeFalse();
            validateReadingResponse.MeterReading.ShouldBeNull();
            _mockAccountRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("12345678")]
        public async Task ValidateReading_returns_invalid_response_when_reading_value_length_exceeds_required_length(string readingValue)
        {
            var account = _fixture.Create<Account>();
            _mockAccountRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(account);
            var meterReadingModel = _fixture.Create<MeterReadingModel>();
            meterReadingModel.ReadingValue = readingValue;
            var sut = CreateSut();

            var validateReadingResponse = await sut.ValidateReading(meterReadingModel);

            validateReadingResponse.IsValid.ShouldBeFalse();
            validateReadingResponse.MeterReading.ShouldBeNull();
            _mockAccountRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task ValidateReading_returns_invalid_response_when_reading_exists()
        {
            var account = _fixture.Create<Account>();
            _mockAccountRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(account);
            _mockMapper
                .Setup(mapper => mapper.Map<MeterReading>(It.IsAny<MeterReadingModel>()))
                .Returns(_fixture.Create<MeterReading>());
            var existingReading = _fixture.Create<MeterReading>();
            _mockMeterReadingRepository
                .Setup(repo => repo.GetExistingReading(It.IsAny<MeterReading>()))
                .ReturnsAsync(existingReading);
            var meterReadingModel = _fixture.Create<MeterReadingModel>();
            meterReadingModel.ReadingValue = "12345";
            var sut = CreateSut();

            var validateReadingResponse = await sut.ValidateReading(meterReadingModel);

            validateReadingResponse.IsValid.ShouldBeFalse();
            validateReadingResponse.MeterReading.ShouldBeNull();
            _mockAccountRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<MeterReading>(It.IsAny<MeterReadingModel>()), Times.Once);
            _mockMeterReadingRepository.Verify(repo => repo.GetExistingReading(It.IsAny<MeterReading>()), Times.Once);
        }

        [Fact]
        public async Task ValidateReading_returns_valid_response_when_all_validation_passes()
        {
            var account = _fixture.Create<Account>();
            _mockAccountRepository
                .Setup(repo => repo.GetById(It.IsAny<int>()))
                .ReturnsAsync(account);
            _mockMapper
                .Setup(mapper => mapper.Map<MeterReading>(It.IsAny<MeterReadingModel>()))
                .Returns(_fixture.Create<MeterReading>());
            MeterReading existingReading = null;
            _mockMeterReadingRepository
                .Setup(repo => repo.GetExistingReading(It.IsAny<MeterReading>()))
                .ReturnsAsync(existingReading);
            var meterReadingModel = _fixture.Create<MeterReadingModel>();
            meterReadingModel.ReadingValue = "12345";
            var sut = CreateSut();

            var validateReadingResponse = await sut.ValidateReading(meterReadingModel);

            validateReadingResponse.IsValid.ShouldBeTrue();
            validateReadingResponse.MeterReading.ShouldNotBeNull();
            _mockAccountRepository.Verify(repo => repo.GetById(It.IsAny<int>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<MeterReading>(It.IsAny<MeterReadingModel>()), Times.Once);
            _mockMeterReadingRepository.Verify(repo => repo.GetExistingReading(It.IsAny<MeterReading>()), Times.Once);
        }

        private MeterReadingValidator CreateSut()
        {
            return new MeterReadingValidator(_mockAccountRepository.Object, _mockMeterReadingRepository.Object, _mockMapper.Object);
        }
    }
}