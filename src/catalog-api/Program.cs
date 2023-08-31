using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CosmosDBContext>();
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

var app = builder.Build();

app.UseCors();

app.MapGet("/products", async (CosmosDBContext database) => {
    // Check the following links for queries: https://learn.microsoft.com/en-us/ef/core/providers/cosmos/?tabs=dotnet-core-cli#queries
    var products = await database.Products.ToListAsync();
    return Results.Ok(products);
});

app.MapGet("/products/{id}", async (string id, CosmosDBContext database) => {
    var product = await database.Products.WithPartitionKey(id).FirstOrDefaultAsync();

    return product == null
        ? Results.NotFound()
        : Results.Ok(product);
});

app.Run();
