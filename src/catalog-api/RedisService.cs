using Microsoft.Azure.StackExchangeRedis;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

public interface IRedisService
{
    Task<Product?> GetProductAsync(string id);
    Task SetProductAsync(Product product);
}

public class RedisService : IRedisService
{ 
    private readonly ConnectionMultiplexer connectionMultiplexer;

    public RedisService(IConfiguration configuration)
    {
        connectionMultiplexer = ConnectionMultiplexer.Connect(configuration["AZURE_REDIS_CONNECTION_STRING"]!, AzureCacheForRedis.ConfigureForAzure);
    }

    private IDatabase Database
    {
        get => connectionMultiplexer.GetDatabase();
    }

    private async Task<string?> Get(string key)
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

    private async Task Set(string key, string value)
    {
        await Database.StringSetAsync(key, value);
    }

    private string ProductKey(string productId)
    {
        return $"products:{productId}";
    }

    public async Task<Product?> GetProductAsync(string id)
    {
        var json = await Get(ProductKey(id));

        if (json == null) {
            return null;
        }

        return JsonSerializer.Deserialize<Product>(json);
    }

    public async Task SetProductAsync(Product product)
    {
        var json = JsonSerializer.Serialize<Product>(product);
        await Set(ProductKey(product.Id), json);
    }
}