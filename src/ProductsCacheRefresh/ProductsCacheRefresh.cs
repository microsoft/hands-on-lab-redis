using System.Collections;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Microsoft.Azure.WebJobs.Extensions.Redis.Samples
{
    public static class ProductsCacheRefresh
    {
        [FunctionName("ProductsEvents")]
        public static void KeyspaceTrigger(
            [RedisPubSubTrigger("REDIS_CONNECTION_STRING", "__keyspace@0__:%REDIS_PRODUCT_ALL%")] ChannelMessage channelMessage,
            ILogger logger)
        {
            logger.LogInformation($"{channelMessage.Channel} => {channelMessage.Message}");
            
            if(channelMessage.Message == "expired")
            {
                logger.LogInformation("EXPIRED");
                // TODO: CALL API TO REFRESH CACHE
            }
        }
    }
}