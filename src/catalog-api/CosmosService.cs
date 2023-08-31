using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

public interface ICosmosService
{
    Task<IEnumerable<Product>> RetrieveAllProductsAsync();
    Task<Product?> RetrieveProductByIdAsync(string id);
}

public class CosmosService : ICosmosService
{ 
    private readonly CosmosClient _client;
    private readonly string databaseName;

    public CosmosService(IConfiguration configuration)
    {
        _client = new CosmosClient(
            connectionString: configuration["AZURE_COSMOS_CONNECTION_STRING"]!
        );
        databaseName = configuration["AZURE_COSMOS_DATABASE"]!;
    }

    private Container productContainer
    {
        get => _client.GetDatabase(databaseName).GetContainer("products");
    }

    public async Task<IEnumerable<Product>> RetrieveAllProductsAsync()
    { 
        var queryable = productContainer.GetItemLinqQueryable<Product>();

        using FeedIterator<Product> feed = queryable
            .OrderByDescending(p => p.Id)
            .ToFeedIterator();

        List<Product> results = new();

        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<Product?> RetrieveProductByIdAsync(string id)
    {
        try
        {
            return await productContainer.ReadItemAsync<Product>(id, new PartitionKey(id));
        }
        catch
        {
            return null;
        }
    }
}