```csharp
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Microsoft.Azure.Functions.Worker.Redis;

namespace Microsoft.Azure.Functions.Worker.Redis.Samples
{
    public static class SampleRedisSamples
    {   
        private readonly ILogger _logger;
        private const string REDIS_CONNECTION_STRING = "REDIS_CONNECTION_STRING";

        public RefreshRedisCache(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RefreshRedisCache>();
        }

        [Description("This function will be triggered when a message is published to the pubsubTest channel.")]
        [Function(nameof(PubSubTrigger))]
        public static void PubSubTrigger(
            [RedisPubSubTrigger(REDIS_CONNECTION_STRING, "PubSubChannelSample")] string message)
        {
            _logger.LogInformation(message);
        }

        [Description("This function will be triggered when a message is published to the KeySpaceTest key.")]
        [Function(nameof(KeyspaceTrigger))]
        public static void KeyspaceTrigger(
            [RedisPubSubTrigger(REDIS_CONNECTION_STRING, "__keyspace@0__:KeySample")] string message)
        {
            _logger.LogInformation(message);

        }

        [Description("This function will be triggered when the DEL command is being executed")]
        [Function(nameof(KeyeventTrigger))]
        public static void KeyeventTrigger(
            [RedisPubSubTrigger(REDIS_CONNECTION_STRING, "__keyevent@0__:expired")] string message)
        {
            _logger.LogInformation(message);
        }

        [Description("This function will be triggered when a change is being made to the listTest list.")]
        [Function(nameof(ListTrigger))]
        public static void ListTrigger(
            [RedisListTrigger(REDIS_CONNECTION_STRING, "SampleList")] string entry)
        {
            _logger.LogInformation(entry);
        }

        [Description("This function will be triggered when a change is being made to the StreamTest Stream.")]
        [Function(nameof(StreamTrigger))]
        public static void StreamTrigger(
            [RedisStreamTrigger(REDIS_CONNECTION_STRING, "SampleStream")] string entry)
        {
            _logger.LogInformation(entry);
        }
    }
}
```