using Azure;
using Azure.Data.Tables;
using Azure.Identity;
using CafeReadConf.Frontend.Models;
using CafeReadConf.Frontend.Service;
using Microsoft.Extensions.Configuration;
using System;

namespace CafeReadConf
{
    public class TableStorageService : IUserService
    {
        private const string TableName = "users";
        private readonly string? _tableStorageConnectionString;
        private readonly string? _tableStorageUri;

        public TableStorageService(IConfiguration configuration)
        {
            _tableStorageConnectionString = configuration.GetValue<string>("secret");
            _tableStorageUri = configuration.GetValue<string>("AZURE_TABLE_STORAGE_URI");
        }

        /// <summary>
        /// Get TableClient from Azure Table Storage
        /// </summary>
        /// <returns></returns>
        private TableClient GetTableClient()
        {
            TableServiceClient serviceClient;

            if(string.IsNullOrEmpty(_tableStorageConnectionString)) // mode MSI
            {
                serviceClient = new TableServiceClient(
                    new Uri(_tableStorageUri),
                    new DefaultAzureCredential());
            }
            else // mode connection string
            {
                serviceClient = new TableServiceClient(_tableStorageConnectionString);
            }

            var tableClient = serviceClient.GetTableClient(TableName);
            return tableClient;
        }


        /// <summary>
        /// Get all users from Azure Table Storage
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserFromApi>> GetUsers()
        {
            var users = new List<UserFromApi>();

            try
            {
                var tableClient = GetTableClient();
                users = tableClient.Query<UserFromApi>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return users;
        }
    }
}
