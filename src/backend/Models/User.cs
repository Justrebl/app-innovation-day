using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace CafeReadConf.Backend.Models
{
    public class UserEntity : ITableEntity
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastname")]
        [JsonInclude]
        public string LastName { get; set; }

        [JsonPropertyName("partitionkey")]
        [JsonInclude]
        public string PartitionKey { get; set; }

        [JsonPropertyName("rowkey")]
        public string? RowKey { get; set; }

        [JsonPropertyName("timestamp")]
        [JsonInclude]
        public DateTimeOffset? Timestamp { get; set; }

        [JsonPropertyName("etag")]
        [JsonInclude]
        public ETag ETag { get; set; }

        public UserEntity(string firstName, string lastName, string partitionKey, string? rowKey, DateTimeOffset? timestamp, ETag eTag)
        {
            FirstName = firstName;
            LastName = lastName;
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Timestamp = timestamp;
            ETag = eTag;
        }
    }

    public class UserEntityFactory
{
    private readonly IConfiguration _configuration;

    public UserEntityFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public UserEntity CreateUserEntity(string firstName, string lastName, string? rowKey = null, DateTimeOffset? timestamp = null, ETag? eTag = null)
    {
        return new UserEntity(
                firstName, 
                lastName, 
                _configuration.GetValue<string>("AZURE_TABLE_PARTITION_KEY"),
                rowKey ?? System.Guid.NewGuid().ToString(),
                timestamp ?? DateTimeOffset.Now,
                eTag ?? ETag.All);
    }
}

}

