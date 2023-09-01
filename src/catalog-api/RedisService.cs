using System;
using Microsoft.Azure.StackExchangeRedis;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class RedisService
{ 
    // TODO: provide a sample with AAD authentication
    private static readonly ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("AZURE_REDIS_CONNECTION_STRING")!, AzureCacheForRedis.ConfigureForAzure);

    private static IDatabase Database
    {
        get => connectionMultiplexer.GetDatabase();
    }

    public static async Task<string?> Get(string key)
    {
        try
        {
            var value = await Database.StringGetAsync(key);
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

    public static async Task Set(string key, string value)
    {
        // TODO: Allow controlling TTL, for the function trigger lab
        await Database.StringSetAsync(key, value);
    }
}