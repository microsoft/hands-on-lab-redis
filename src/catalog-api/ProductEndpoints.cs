public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
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
    }
}