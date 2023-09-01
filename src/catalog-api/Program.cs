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
builder.Services.AddSingleton<IProductCacheService, ProductCacheService>();

var app = builder.Build();
app.UseCors();

app.MapProductEndpoints();

app.Run();
