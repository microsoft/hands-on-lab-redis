using System.Text.Json;

public interface IProductCacheService
{
    Task<Product?> GetProductAsync(string id);
    Task SetProductAsync(Product product);
    Task<IEnumerable<Product>?> GetProductsAsync();
    Task SetProductsAsync(IEnumerable<Product> products);
}

public class ProductCacheService : IProductCacheService
{
    private readonly IRedisService _redisService;
    private readonly string _prefix = "products";
    private readonly bool _disableProductListCache;

    public ProductCacheService(IRedisService redisService, IConfiguration configuration)
    {
        _redisService = redisService;
        _disableProductListCache = configuration["CACHE_DISABLE"] == "1";
    }

    private string ProductKey(string productId) => $"{_prefix}:{productId}";

    public async Task<Product?> GetProductAsync(string id)
    {
        var json = await _redisService.Get(ProductKey(id));

        if (json == null) {
            return null;
        }

        return JsonSerializer.Deserialize<Product>(json);
    }

    public async Task SetProductAsync(Product product)
    {
        var json = JsonSerializer.Serialize<Product>(product);
        await _redisService.Set(ProductKey(product.Id), json);
    }

    public async Task<IEnumerable<Product>?> GetProductsAsync()
    {
        if (_disableProductListCache) {
            return null;
        }

        var json = await _redisService.Get("products");

        if (json == null) {
            return null;
        }

        return JsonSerializer.Deserialize<List<Product>>(json);
    }

    public async Task SetProductsAsync(IEnumerable<Product> products)
    {
        if (!_disableProductListCache) {
            var json = JsonSerializer.Serialize<IEnumerable<Product>>(products);
            await _redisService.Set(_prefix, json);
        }
    }
}