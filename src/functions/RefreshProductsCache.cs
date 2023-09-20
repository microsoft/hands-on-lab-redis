using System.Net;
using System.Collections;
using Microsoft.Extensions.Http;
using System.ComponentModel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Services.Redis;

namespace functions
{
    public class RefreshProductsCache
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient = null!;
        private readonly IRedisService _redisService;
        private readonly string _KEY_PRODUCTS_ALL; 

        public RefreshProductsCache(
            IHttpClientFactory httpClientFactory,
            ILoggerFactory loggerFactory, 
            IRedisService redisService)
        {
            _httpClient = httpClientFactory.CreateClient("catalogApi");
            _logger = loggerFactory.CreateLogger<RefreshProductsCache>();
            _redisService = redisService;
            _KEY_PRODUCTS_ALL = Environment.GetEnvironmentVariable("REDIS_KEY_PRODUCTS_ALL");
        }

        [Description("This function will be triggered when the EXPIRED command is being executed at monitoredKey expiry")]
        [Function("ProductsEvents")]
        public async Task ProductsEventsTrigger(
            [RedisPubSubTrigger("AZURE_REDIS_CONNECTION_STRING", "__keyevent@0__:expired")] string key)
        {
            var keyChanged = key.Split("_")[1];
            if(keyChanged == _KEY_PRODUCTS_ALL)
            {
                _logger.LogInformation($"{key} just EXPIRED"); 
                
                var result = await _httpClient.GetStringAsync("products");
                _redisService.Set(key, result);
            }
        }
    }
}