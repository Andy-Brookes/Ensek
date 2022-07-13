namespace Ensek.Test.Domain.Entities
{
    public class MeterReading
    {
        public int Id { get; set; }

        public DateTime ReadingDateTime { get; set; }

        public int ReadingValue { get; set; }

        public int AccountId { get; set; }
    }
}