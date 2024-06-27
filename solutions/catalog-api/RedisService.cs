using StackExchange.Redis;

public interface IRedisService {
    Task<string?> Get(string key);
    Task Set(string key, string value);
    Task AddToStream(string streamName, Dictionary<string,string?> data);
}

public class RedisService : IRedisService
{ 
    private IDatabase? _database = null;
    private readonly string? _connectionString;
    private readonly string? _hostname;
    private readonly string? _port;

    private readonly int _defaultTTLInSeconds = 60;
    private readonly TimeSpan _ttl; // Time To Live

    public RedisService(IConfiguration configuration)
    {
        _ttl = TTL(configuration["AZURE_REDIS_TTL_IN_SECONDS"]);
        _connectionString = configuration["AZURE_REDIS_CONNECTION_STRING"];
        _port = configuration["AZURE_REDIS_PORT"];
        _hostname = configuration["AZURE_REDIS_HOSTNAME"];
    }

    private async Task<IDatabase> GetDatabaseAsync() {
        if (_database != null) {
            return _database;
        }

        Console.WriteLine("Initializing Redis database connection");

        // var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_connectionString!, AzureCacheForRedis.ConfigureForAzure);
        var configurationOptions = await ConfigurationOptions.Parse($"{_hostname}:{_port}").ConfigureForAzureWithSystemAssignedManagedIdentityAsync();
        var connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(configurationOptions);

        _database = connectionMultiplexer.GetDatabase();

        Console.WriteLine("Redis database connection established");

        return _database;
    }

    private TimeSpan TTL(string? ttlInSecondsAsString)
    {
        int ttlInSeconds;

        try
        {
            ttlInSeconds = string.IsNullOrEmpty(ttlInSecondsAsString) ? _defaultTTLInSeconds : int.Parse(ttlInSecondsAsString);
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
            var database = await GetDatabaseAsync();
            var value = await database.StringGetAsync(key);
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
        var database = await GetDatabaseAsync();
        await database.StringSetAsync(key, value, _ttl);
    }

    public async Task AddToStream(string streamName, Dictionary<string,string?> data)
    {
        List<NameValueEntry> entries = new();

        foreach(KeyValuePair<string, string?> keyValuePair in data)
        {
            entries.Add(new(keyValuePair.Key, keyValuePair.Value));
        }

        var database = await GetDatabaseAsync();
        await database.StreamAddAsync(streamName, entries.ToArray());
    }
}