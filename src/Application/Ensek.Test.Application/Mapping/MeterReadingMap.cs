using CsvHelper.Configuration;
using Ensek.Test.Application.Models;

namespace Ensek.Test.Application.Mapping
{
    public class MeterReadingMap : ClassMap<MeterReadingModel>
    {
        public MeterReadingMap()
        {
            Map(meterReading => meterReading.AccountId).Name("AccountId");
            Map(meterReading => meterReading.ReadingDateTime).Convert(args => DateTime.Parse(args.Row.GetField("MeterReadingDateTime")));
            Map(meterReading => meterReading.ReadingValue).Name("MeterReadValue");
        }
    }
}
