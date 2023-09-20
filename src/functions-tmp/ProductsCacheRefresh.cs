using System.Collections;
using System.ComponentModel;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Functions
{
    public static class RedisCacheRefresh
    {
        [Description("This function will be triggered when the DEL command is being executed")]
        [FunctionName("ProductsEvents")]
        public static void ProductsEventsTrigger(
            [RedisPubSubTrigger("REDIS_CONNECTION_STRING", "__keyspace@0__:*_%REDIS_PRODUCT_ALL%")] ChannelMessage channelMessage,
            ILogger logger)
        {
            logger.LogInformation($"{channelMessage.Channel} => {channelMessage.Message}");
            if(channelMessage.Message == "expired")
            {
                logger.LogInformation("EXPIRED"); 
                // TODO: CALL API TO REFRESH CACHE HERE 
            }
        }
    }
}