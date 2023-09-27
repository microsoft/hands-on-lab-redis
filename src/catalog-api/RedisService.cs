using System.Diagnostics;
using StackExchange.Redis;

public interface IRedisService {
    Task<string?> Get(string key);
    Task Set(string key, string value);
    Task AddToStream(string streamName, Dictionary<string,string?> data);
}

public class RedisService : IRedisService
{ 
    // TODO: provide a sample with AAD authentication
    private readonly IDatabase _database;
    private readonly int _defaultTTLInSeconds = 60;
    private readonly TimeSpan _ttl; // Time To Live

    private readonly Stopwatch _stopwatch;

    public RedisService(IConfiguration configuration)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration["AZURE_REDIS_CONNECTION_STRING"], AzureCacheForRedis.ConfigureForAzure);
        _database = connectionMultiplexer.GetDatabase();
        _ttl = TTL(configuration["AZURE_REDIS_TTL_IN_SECONDS"]);
        _stopwatch = new Stopwatch();
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
            _stopwatch.Start();
            
            var value = await _database.StringGetAsync(key);

            _stopwatch.Stop();

            Console.WriteLine($"GET | Call to Redis elapsed time : {_stopwatch.ElapsedMilliseconds} ms");

            _stopwatch.Reset();

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
        _stopwatch.Start();

        await _database.StringSetAsync(key, value, _ttl);
        
        _stopwatch.Stop();
        Console.WriteLine($"SET | Call to Redis elapsed time : {_stopwatch.ElapsedMilliseconds} ms");
        _stopwatch.Reset();        
    }

    public async Task AddToStream(string streamName, Dictionary<string,string?> data)
    {
        List<NameValueEntry> entries = new();

        foreach(KeyValuePair<string, string?> keyValuePair in data)
        {
            entries.Add(new(keyValuePair.Key, keyValuePair.Value));
        }

        await _database.StreamAddAsync(streamName, entries.ToArray());
    }
}