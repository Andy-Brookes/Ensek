using Ensek.Test.Application.Mapping;
using Ensek.Test.Application.Models;
using Shouldly;

namespace Ensek.Test.Application.Tests.Mapping
{
    public class MeterReadingMapTests
    {
        [Fact]
        public void MeterReadingMap_maps_expected_properties()
        {
            const int NumberOfMappedProperties = 3;
            var sut = CreateSut();

            sut.MemberMaps.Count().ShouldBe(NumberOfMappedProperties);
            sut.MemberMaps.Find<MeterReadingModel>(model => model.AccountId).ShouldNotBeNull();
            sut.MemberMaps.Find<MeterReadingModel>(model => model.ReadingDateTime).ShouldNotBeNull();
            sut.MemberMaps.Find<MeterReadingModel>(model => model.ReadingValue).ShouldNotBeNull();
        }

        private static MeterReadingMap CreateSut()
        {
            return new MeterReadingMap();
        }
    }
}
