using System.Net;
using System.Collections;
using Microsoft.Extensions.Http;
using System.ComponentModel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Redis;
using Microsoft.Extensions.Logging;

namespace Func.RedisCache.Products;
public class RefreshProductsCache
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpCatalogApiClient;

    public RefreshProductsCache(
        IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory)
    {
        _httpCatalogApiClient = httpClientFactory.CreateClient(Const.CATALOG_API_CLIENT);
        _logger = loggerFactory.CreateLogger<RefreshProductsCache>();
    }

    [Description("This function will be triggered when the EXPIRED command is executed at monitored key's expiry.")]
    [Function("ProductsEvents")]
    public async Task ProductsEventsTrigger(
        [RedisPubSubTrigger("AZURE_REDIS_CONNECTION_STRING", "__keyevent@0__:expired")] string key)
    {
        if (key.Contains(Const.REDIS_KEY_PRODUCTS_ALL))
        {
            _logger.LogInformation($"{key} just EXPIRED");

            //Fire and forget : Calling APIM to request fresh product catalog from data source after cache expired
            await _httpCatalogApiClient.GetStringAsync("products");
            
            _logger.LogInformation($"called APIM to force Redis refresh key '{key}' with fresh product catalog from data source.");
        }
    }
}
