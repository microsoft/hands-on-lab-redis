using System;
using Microsoft.Azure.StackExchangeRedis;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface IRedisService {
    Task<string?> Get(string key);
    Task Set(string key, string value);
}

public class RedisService : IRedisService
{ 
    // TODO: provide a sample with AAD authentication
    private readonly IDatabase _database;
    private readonly TimeSpan? _ttl; // Time To Live

    public RedisService(IConfiguration configuration)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration["AZURE_REDIS_CONNECTION_STRING"], AzureCacheForRedis.ConfigureForAzure);
        _database = connectionMultiplexer.GetDatabase();
        _ttl = TTL(configuration["AZURE_REDIS_TTL_IN_SECONDS"]);
    }

    private TimeSpan? TTL(string? ttlInSecondsAsString)
    {
        try
        {
            int ttlInSeconds = String.IsNullOrEmpty(ttlInSecondsAsString) ? 0 : Int32.Parse(ttlInSecondsAsString);

            if (ttlInSeconds <= 0) {
                return null;
            }

            return TimeSpan.FromSeconds(ttlInSeconds);
        }
        catch
        {
            return null;
        }
    }

    public async Task<string?> Get(string key)
    {
        try
        {
            var value = await _database.StringGetAsync(key);
            var stringValue = value.ToString();

            if (stringValue == string.Empty) {
                return null;
            }

            return stringValue;
        }
        catch
        {
            return null;
        }
    }

    public async Task Set(string key, string value)
    {
        await _database.StringSetAsync(key, value, _ttl);
    }
}