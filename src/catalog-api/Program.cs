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

builder.Services.AddSingleton<ISimulatedDatabaseLatency, SimulatedDatabaseLatency>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<ICosmosService, CosmosService>();
builder.Services.AddScoped<IProductCacheService, ProductCacheService>();
builder.Services.AddScoped<IBrowsingHistoryCacheService, BrowsingHistoryCacheService>();

var app = builder.Build();
app.UseCors();

app.MapProductEndpoints();

app.Run();
