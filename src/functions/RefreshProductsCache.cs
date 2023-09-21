using System.Net;
using System.Collections;
using Microsoft.Extensions.Http;
using System.ComponentModel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Services.Redis;

namespace functions;
public class RefreshProductsCache
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpCatalogApiClient;
    private readonly IRedisService _redisService;

    public RefreshProductsCache(
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory,
        IRedisService redisService)
    {
        _httpCatalogApiClient = httpClientFactory.CreateClient(Const.CATALOG_API_CLIENT);
        _logger = loggerFactory.CreateLogger<RefreshProductsCache>();
        _redisService = redisService;
    }

    [Description("This function will be triggered when the EXPIRED command is being executed at monitoredKey's expiry")]
    [Function("ProductsEvents")]
    public async Task ProductsEventsTrigger(
        [RedisPubSubTrigger("AZURE_REDIS_CONNECTION_STRING", "__keyevent@0__:expired")] string key)
    {
        if (key.Contains(Const.REDIS_KEY_PRODUCTS_ALL))
        {
            _logger.LogInformation($"{key} just EXPIRED");

            var result = await _httpCatalogApiClient.GetStringAsync("products");
            _redisService.Set(key, result);
        }
    }
}
