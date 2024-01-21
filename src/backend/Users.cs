using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace backend
{
    public class CreateUserOutput{
        [TableOutput("%AZURE_TABLE_SOURCE%", Connection = "AZURE_TABLE_STORAGE_ACCOUNT")]
        public UserEntity User {get;set;}
        public HttpResponseMessageData Response {get;set;}
    }

    public class Users
    {
        private readonly ILogger _logger;

        public Users(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Users>();
        }

        [Function("Users")]
        public CreateUserOutput Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return new CreateUserOutput(){
                User = new UserEntity(){
                    PartitionKey = "users",
                    RowKey = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    LastName = "Doe",
                    Timestamp = DateTimeOffset.UtcNow
                    },
                Response = response
            }
        }
    }
}
