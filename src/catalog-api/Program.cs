var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});
builder.Services.AddSingleton<ICosmosService, CosmosService>();
builder.Services.AddSingleton<IRedisService, RedisService>();

var app = builder.Build();
app.UseCors();

app.MapGet("/products", async (ICosmosService cosmosService) => {
    // Check the following links for queries: https://learn.microsoft.com/en-us/ef/core/providers/cosmos/?tabs=dotnet-core-cli#queries
    var products = await cosmosService.RetrieveAllProductsAsync();
    return Results.Ok(products);
});

app.MapGet("/products/{id}", async (ICosmosService cosmosService, IRedisService redisService, string id) => {
    Product? cachedProduct = await redisService.GetProductAsync(id);

    if (cachedProduct != null) {
        Console.WriteLine("Returning a production description from the cache:");
        Console.WriteLine(cachedProduct?.Id);

        return Results.Ok(cachedProduct);
    }

    var product = await cosmosService.RetrieveProductByIdAsync(id);

    if (product == null) {
        return Results.NotFound();
    }

    await redisService.SetProductAsync(product);

    return Results.Ok(product);
});

app.Run();
