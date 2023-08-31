using System;

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

var app = builder.Build();
app.UseCors();

app.MapGet("/products", async (ICosmosService cosmosService) => {
    // Check the following links for queries: https://learn.microsoft.com/en-us/ef/core/providers/cosmos/?tabs=dotnet-core-cli#queries
    var products = await cosmosService.RetrieveAllProductsAsync();
    return Results.Ok(products);
});

app.MapGet("/products/{id}", async (ICosmosService cosmosService, string id) => {
    var product = await cosmosService.RetrieveProductByIdAsync(id);

    return product == null
        ? Results.NotFound()
        : Results.Ok(product);
});

app.Run();
