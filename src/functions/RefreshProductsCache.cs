using System.Net;
using System.Collections;
using System.ComponentModel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace functions
{
    public class RefreshProductsCache
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public RefreshProductsCache(
            HttpClient httpClient,
            ILoggerFactory loggerFactory)
        {
            _httpClient = httpClient;
            _logger = loggerFactory.CreateLogger<RefreshProductsCache>();
        }

        [Description("This function will be triggered when the EXPIRED command is being executed at monitoredKey expiry")]
        [FunctionName("ProductsEvents")]
        public static void ProductsEventsTrigger(
            [RedisPubSubTrigger("REDIS_CONNECTION_STRING", "__keyspace@0__:*_%REDIS_KEY_PRODUCTS_ALL%")] ChannelMessage channelMessage)
        {
            _logger.LogInformation($"{channelMessage.Channel} => {channelMessage.Message}");
            if(channelMessage.Message == "expired")
            {
                _logger.LogInformation("EXPIRED"); 
                
                var result = await _httpClient.GetAsync("/products")
                    .GetAwaiter()
                    .GetResult();
                // TODO: CALL API TO REFRESH CACHE HERE 
            }
        }
    }
}