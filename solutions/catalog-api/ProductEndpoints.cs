public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/products", async (ICosmosService cosmosService, IProductCacheService productCacheService) => {
            IEnumerable<Product>? cachedProducts = await productCacheService.GetProductsAsync();

            if (cachedProducts != null) {
                Console.WriteLine("Returning product list from the cache");
                return Results.Ok(cachedProducts);
            }

            // Fetch data from Cosmos DB
            var products = await cosmosService.RetrieveAllProductsAsync();

            if (products.Any()) {
                await productCacheService.SetProductsAsync(products);
            }

            return Results.Ok(products);
        });

        app.MapGet("/products/{id}", async (ICosmosService cosmosService, IProductCacheService productCacheService, IBrowsingHistoryCacheService browsingHistoryCacheService, string id, [FromHeader(Name = "X-USER-ID")] string? userId) => {
            Product? cachedProduct = await productCacheService.GetProductAsync(id);

            if (cachedProduct != null) {
                Console.WriteLine("Returning a product description from the cache:");
                Console.WriteLine(cachedProduct?.Id);

                await browsingHistoryCacheService.AddViewedProductAsync(userId, cachedProduct);

                return Results.Ok(cachedProduct);
            }

            var product = await cosmosService.RetrieveProductByIdAsync(id);

            if (product == null) {
                return Results.NotFound();
            }

            await productCacheService.SetProductAsync(product);
            await browsingHistoryCacheService.AddViewedProductAsync(userId, product);

            return Results.Ok(product);
        });
    }
}