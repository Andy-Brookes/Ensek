using AutoFixture;
using Ensek.Test.Application.Models;
using Ensek.Test.Domain.Entities;
using Shouldly;

namespace Ensek.Test.Application.Tests.Models
{
    public class ValidateReadingResponseTests
    {
        [Fact]
        public void CreateInvalidResponse_returns_instance_with_expected_values()
        {
            var invalidResponse = ValidateReadingResponse.CreateInvalidResponse();

            invalidResponse.IsValid.ShouldBeFalse();
            invalidResponse.MeterReading.ShouldBeNull();
        }

        [Fact]
        public void CreateValidResponse_returns_instance_with_expected_values()
        {
            var meterReading = new Fixture().Create<MeterReading>();

            var invalidResponse = ValidateReadingResponse.CreateValidResponse(meterReading);

            invalidResponse.IsValid.ShouldBeTrue();
            invalidResponse.MeterReading.ShouldBe(meterReading);
        }
    }
}