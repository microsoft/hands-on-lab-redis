```csharp
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Microsoft.Azure.WebJobs.Extensions.Redis.Samples
{
    public static class SampleRedisSamples
    {
        
        public const string REDIS_CONNECTION_STRING = "REDIS_CONNECTION_STRING";

        [Description("This function will be triggered when a message is published to the pubsubTest channel.")]
        [FunctionName(nameof(PubSubTrigger))]
        public static void PubSubTrigger(
            [RedisPubSubTrigger(REDIS_CONNECTION_STRING, "PubSubChannelSample")] string message,
            ILogger logger)
        {
            logger.LogInformation(message);
        }

        [Description("This function will be triggered when a message is published to the KeySpaceTest key.")]
        [FunctionName(nameof(KeyspaceTrigger))]
        public static void KeyspaceTrigger(
            [RedisPubSubTrigger(REDIS_CONNECTION_STRING, "__keyspace@0__:KeySample")] string message,
            ILogger logger)
        {
            logger.LogInformation(message);

        }

        [Description("This function will be triggered when the DEL command is being executed")]
        [FunctionName(nameof(KeyeventTrigger))]
        public static void KeyeventTrigger(
            [RedisPubSubTrigger(REDIS_CONNECTION_STRING, "__keyevent@0__:expired")] string message,
            ILogger logger)
        {
            logger.LogInformation(message);
        }

        [Description("This function will be triggered when a change is being made to the listTest list.")]
        [FunctionName(nameof(ListTrigger))]
        public static void ListTrigger(
            [RedisListTrigger(REDIS_CONNECTION_STRING, "SampleList")] string entry,
            ILogger logger)
        {
            logger.LogInformation(entry);
        }

        [Description("This function will be triggered when a change is being made to the StreamTest Stream.")]
        [FunctionName(nameof(StreamTrigger))]
        public static void StreamTrigger(
            [RedisStreamTrigger(REDIS_CONNECTION_STRING, "SampleStream")] string entry,
            ILogger logger)
        {
            logger.LogInformation(entry);
        }
    }
}
```