using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Redis;
using Microsoft.Extensions.DependencyInjection;
using functions;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services)=> {
        services.AddHttpClient<RefreshProductsCache>(
            client => {
                client.BaseAddress = new Uri(hostContext.Configuration["CATALOG_API_URL"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }
        )
        .SetHandlerLifetime(TimeSpan.FromMinutes(10));
    })
    .Build();

// //Retrieve the product catalog URL from the environment variables
// string? productCatalogUrl= host.Configuration["CATALOG_API_URL"];
// //Fail fast in case the product catalog URL is not set
// ArgumentException.ThrowIfNullOrEmpty(productCatalogUrl);

// host.ConfigureServices(services => {
    
    
// });

host.Run();
