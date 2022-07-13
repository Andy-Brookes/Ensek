namespace Ensek.Test.Application.Models
{
    public class MeterReadingModel
    {
        public int AccountId { get; set; }

        public DateTime ReadingDateTime { get; set; }

        public string? ReadingValue { get; set; }
    }
}
