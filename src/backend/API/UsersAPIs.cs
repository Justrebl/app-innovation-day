using System.Net;
using System.Text.Json;
using CafeReadConf.Backend.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CafeReadConf.Backend.API
{
    public class CreateUserOutput{
        [TableOutput("%AZURE_TABLE_SOURCE%", Connection = "AZURE_TABLE_STORAGE_ACCOUNT")]
        public UserEntity User {get;set;}
        public HttpResponseData Response {get;set;}

        public CreateUserOutput(UserEntity user, HttpResponseData response){
            User = user;
            Response = response;
        }
    }

    public class UsersAPI
    {
        private readonly ILogger _logger;
        private readonly string _partitionkey;
        private readonly UserEntityFactory _userEntityFactory;
        private readonly string _sourceTable;

        public UsersAPI(ILoggerFactory loggerFactory, IConfiguration configuration, UserEntityFactory userEntityFactory)
        {
            //Creating singletons and injecting dependencies
            _logger = loggerFactory.CreateLogger<UsersAPI>();
            _userEntityFactory = userEntityFactory;

            //Hydrating configurations from environment variables
            _partitionkey = configuration.GetValue<string>("AZURE_TABLE_PARTITION_KEY");            
            _sourceTable = configuration.GetValue<string>("AZURE_TABLE_SOURCE");
        }

        [Function(nameof(CreateUser))]
        public CreateUserOutput CreateUser(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route ="Users")] HttpRequestData req,
            [FromBody] UserEntity newUser)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString($"Creating a new user : {newUser.FirstName} {newUser.LastName}");

            return new CreateUserOutput(
                _userEntityFactory.CreateUserEntity(newUser.FirstName, newUser.LastName),
                response);
        }

        [Function(nameof(GetUsersById))]
        public HttpResponseData GetUsersById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Users/{userId}")] HttpRequestData req,
            [TableInput(
                tableName: "%AZURE_TABLE_SOURCE%",
                partitionKey: "%AZURE_TABLE_PARTITION_KEY%",
                rowKey: "{userId}",
                Connection = "AZURE_TABLE_STORAGE_ACCOUNT")] UserEntity user,
                int userId)
        {
            _logger.LogInformation("Retrieving user with id: {UserId} in the table : {SourceTable}", userId, _sourceTable);
            
            //Preparing user entity to be returned
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString($"{JsonSerializer.Serialize(user)}");
            
            return response;
        }

        [Function(nameof(GetUsers))]
        public HttpResponseData GetUsers(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Users")] HttpRequestData req,
            [TableInput(
                tableName: "%AZURE_TABLE_SOURCE%",
                partitionKey: "%AZURE_TABLE_PARTITION_KEY%",
                Connection = "AZURE_TABLE_STORAGE_ACCOUNT",
                Take = 50)] IEnumerable<UserEntity> users,
                int userId)
        {
            _logger.LogInformation("Retrieving user with id: {UserId} in the table : {SourceTable}",userId, _sourceTable);
            
            //Preparing user entity to be returned
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString($"{JsonSerializer.Serialize(users)}");
            
            return response;
        }
    }
}
