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


    [Description("This function will be triggered when the EXPIRED command is being executed at monitored key's expiry.")]
    [Function("ProductsEvents")]
    public async Task ProductsEventsTrigger([RedisPubSubTrigger("TO_DEFINE", "TO_DEFINE")] string key)
    {
        // TODO: Implement the logic to refresh the cache
    }
}
