using System;
using Microsoft.Azure.StackExchangeRedis;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services.Redis {
    public interface IRedisService {
    Task<string?> Get(string key);
    Task Set(string key, string value);
}

public class RedisService : IRedisService
{ 
    private readonly IDatabase _database;
    private readonly int _defaultTTLInSeconds = 60;
    private readonly TimeSpan _ttl; // Time To Live

    public RedisService()
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("AZURE_REDIS_CONNECTION_STRING") ?? "", AzureCacheForRedis.ConfigureForAzure);
        _database = connectionMultiplexer.GetDatabase();
        _ttl = TTL(Environment.GetEnvironmentVariable("AZURE_REDIS_TTL_IN_SECONDS"));
    }

    private TimeSpan TTL(string? ttlInSecondsAsString)
    {
        int ttlInSeconds;

        try
        {
            ttlInSeconds = String.IsNullOrEmpty(ttlInSecondsAsString) ? _defaultTTLInSeconds : Int32.Parse(ttlInSecondsAsString);
        }
        catch
        {
            ttlInSeconds = _defaultTTLInSeconds;
        }

        if (ttlInSeconds <= 0) {
            ttlInSeconds = _defaultTTLInSeconds;
        }

        return TimeSpan.FromSeconds(ttlInSeconds);
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
}
