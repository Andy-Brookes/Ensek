namespace Ensek.Test.Application.Models
{
    public class UploadReadingsResponse
    {
        public int FailedReadings { get; set; }

        public int SuccessfulReadings { get; set; }

        public static UploadReadingsResponse Create(int failed, int successful)
        {
            return new UploadReadingsResponse
            {
                FailedReadings = failed,
                SuccessfulReadings = successful
            };
        }
    }
}
