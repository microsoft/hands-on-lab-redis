using System.Collections;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace HoL.Redis.Products
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