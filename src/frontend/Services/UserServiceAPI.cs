using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using CafeReadConf.Frontend.Models;
using CafeReadConf.Frontend.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Text.Json;

namespace CafeReadConf
{
    public class UserServiceAPI : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserServiceAPI(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory) : base(configuration)
        {
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        // /// <summary>
        // /// Get TableClient from Azure Table Storage
        // /// </summary>
        // /// <returns></returns>
        // private TableClient GetTableClient()
        // {
        //     TableServiceClient serviceClient;

        //     if(string.IsNullOrEmpty(_tableStorageConnectionString)) // mode MSI
        //     {
        //         serviceClient = new TableServiceClient(
        //             new Uri(_tableStorageUri),
        //             new DefaultAzureCredential());
        //     }
        //     else // mode connection string
        //     {
        //         serviceClient = new TableServiceClient(_tableStorageConnectionString);
        //     }

        //     var tableClient = serviceClient.GetTableClient(TableName);
        //     return tableClient;
        // }


        /// <summary>
        /// Get all users from Azure Table Storage
        /// </summary>
        /// <returns></returns>
        public override async Task<List<UserEntity>> GetUsers()
        {
            var users = new List<UserEntity>();
            try
            {
                var userApiResult = await _httpClient.GetAsync("/api/users");

                if (userApiResult.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine($"Error: {userApiResult.StatusCode}");
                    return users;
                }

                users = JsonSerializer.Deserialize<List<UserEntity>>(
                    await userApiResult.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return users;
        }
    }
}