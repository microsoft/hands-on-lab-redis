public interface IBrowsingHistoryCacheService
{
    Task AddViewedProductAsync(string? userId, Product product);
}

public class BrowsingHistoryCacheService : IBrowsingHistoryCacheService
{
    private readonly IRedisService _redisService;
    private readonly string _streamName;

    public BrowsingHistoryCacheService(IRedisService redisService, IConfiguration configuration)
    {
        _redisService = redisService;

        string? streamName = configuration["PRODUCT_VIEWS_STREAM_NAME"];
        _streamName = String.IsNullOrEmpty(streamName) ? "productViews" : streamName;
    }

    public async Task AddViewedProductAsync(string? userId, Product product)
    {
        if (userId == null) {
            return;
        }

        var productView = new Dictionary<string, string?>
        {
            ["userId"] = userId,
            ["productId"] = product.Id,
            ["productTitle"] = product.Title,
            ["date"] = DateTime.UtcNow.ToString("o") // Date in ISO-8601
        };

        await _redisService.AddToStream(_streamName, productView);
    }
}