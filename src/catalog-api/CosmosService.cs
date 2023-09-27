using System.Diagnostics;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

public interface ICosmosService
{
    Task<IEnumerable<Product>> RetrieveAllProductsAsync();
    Task<Product?> RetrieveProductByIdAsync(string id);
}

public class CosmosService : ICosmosService
{ 
    private readonly CosmosClient _cosmosClient;
    private readonly ISimulatedDatabaseLatency _simulatedDatabaseLatency;
    private readonly string _databaseName;
    private readonly Stopwatch _stopwatch;

    public CosmosService(IConfiguration configuration, ISimulatedDatabaseLatency simulatedDatabaseLatency)
    {
        _cosmosClient = new CosmosClient(
            connectionString: configuration["AZURE_COSMOS_CONNECTION_STRING"]!
        );
        _simulatedDatabaseLatency = simulatedDatabaseLatency;
        _databaseName = configuration["AZURE_COSMOS_DATABASE"]!;
        _stopwatch = new Stopwatch();
    }

    private Container productContainer
    {
        get => _cosmosClient.GetDatabase(_databaseName).GetContainer("products");
    }

    public async Task<IEnumerable<Product>> RetrieveAllProductsAsync()
    {
        await _simulatedDatabaseLatency.Wait();
        
        _stopwatch.Start();

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

        _stopwatch.Stop();
        
        Console.WriteLine ($"Call to CosmosDb elapsed time : {_stopwatch.ElapsedMilliseconds} ms");

        _stopwatch.Reset();

        return results;
    }

    public async Task<Product?> RetrieveProductByIdAsync(string id)
    {
        await _simulatedDatabaseLatency.Wait();

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