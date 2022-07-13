using Ensek.Test.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

namespace Ensek.Test.Api.Tests.Controllers02111
{
    public class MeterReadingControllerTests
    {
        private readonly HttpClient _httpClient;

        public MeterReadingControllerTests()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            _httpClient = webApplicationFactory.CreateClient();
        }

        [Theory]
        [InlineData("empty.csv", "text/csv", "File must not be empty")]
        [InlineData("test.txt", "text/plain", "File must be a csv")]
        public async Task Post_endpoint_returns_BadRequest_with_expected_error_when_file_is_empty_or_not_a_csv(string sourceFile, string contentType, string expectedErrorMessage)
        {
            var httpResponseMessage = await PostFile(sourceFile, contentType);
            
            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await httpResponseMessage.Content.ReadAsStringAsync());
            problemDetails.Detail.ShouldBe(expectedErrorMessage);
            problemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);
            problemDetails.Title.ShouldBe("Bad Request");
        }

        [Fact]
        public async Task Post_endpoint_returns_status_200_with_UploadReadingsResponse_when_file_is_a_valid_csv()
        {
            var httpResponseMessage = await PostFile("test.csv");

            var uploadReadingsResponse = JsonConvert.DeserializeObject<UploadReadingsResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            uploadReadingsResponse.ShouldNotBeNull();
            httpResponseMessage.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        private async Task<HttpResponseMessage> PostFile(string sourceFile, string contentType = "text/csv")
        {
            ByteArrayContent fileContent;

            if (sourceFile == null)
            {
                fileContent = new ByteArrayContent(Array.Empty<byte>());
            }
            else
            {
                var fileLocation = $"{Assembly.GetExecutingAssembly().Location.Split("\\bin").First()}\\TestData";
                var filename = Path.Combine(fileLocation, sourceFile);
                var fileBytes = await File.ReadAllBytesAsync(filename);
                fileContent = new ByteArrayContent(fileBytes);
            }

            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"File\"",
                FileName = "\"" + sourceFile + "\""
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            var multipartFormDataContent = new MultipartFormDataContent
            {
                fileContent
            };

            return await _httpClient.PostAsync("meterreadings/meter-reading-uploads", multipartFormDataContent);
        }
    }
}