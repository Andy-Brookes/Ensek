using Ensek.Test.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Ensek.Test.Api.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class MeterReadingsController : ControllerBase
    {
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingsController(IMeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        [HttpPost("meter-reading-uploads")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] MeterReadingsFile meterReadingsFile)
        {
            if (EmptyFile(meterReadingsFile.File))
            {
                return Problem("File must not be empty", statusCode: StatusCodes.Status400BadRequest);
            }

            if (InvalidFileType(meterReadingsFile.File))
            {
                return Problem("File must be a csv", statusCode: StatusCodes.Status400BadRequest);
            }

            var uploadReadingsResponse = await _meterReadingService.UploadReadings(meterReadingsFile.File.OpenReadStream());

            return Ok(uploadReadingsResponse);
        }

        private static bool EmptyFile(IFormFile file) => file == null || file.Length == 0;

        private static bool InvalidFileType(IFormFile file) => !file.ContentType.Equals("text/csv");
    }

    public class MeterReadingsFile
    {
        public IFormFile File { get; set; }
    }
}