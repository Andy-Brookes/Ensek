using AutoFixture;
using Ensek.Test.Application.Models;
using Shouldly;

namespace Ensek.Test.Application.Tests.Models
{
    public class UploadReadingResponseTests
    {
        [Fact]
        public void Create_returns_instance_with_expected_values()
        {
            var fixture = new Fixture();
            var failed = fixture.Create<int>();
            var successful = fixture.Create<int>();

            var response = UploadReadingsResponse.Create(failed, successful);

            response.FailedReadings.ShouldBe(failed);
            response.SuccessfulReadings.ShouldBe(successful);
        }
    }
}