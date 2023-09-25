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
        [RedisPubSubTrigger("TO_DEFINE", "TO_DEFINE")] string key)
    {
        // TODO: Implement the logic to refresh the cache
    }
}
