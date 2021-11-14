using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace IdeaMachine.ProfilePictureService
{
    public class Upload
    {
        private readonly ILogger _logger;

        public Upload(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Upload>();
        }

        [Function(nameof(UploadPicture))]
        public HttpResponseData UploadPicture([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
