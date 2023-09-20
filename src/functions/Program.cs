using System;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .Build();

//Retrieve the product catalog URL from the environment variables
string? productCatalogUrl= host.Configuration["CATALOG_API_URL"];
//Fail fast in case the product catalog URL is not set
ArgumentException.ThrowIfNullOrEmpty(productCatalogUrl);

host.services.ConfigureServices(services => {
    services.AddHttpClient<RefreshProductsCache>(
        client => {
            client.BaseAddress = new Uri(productCatalogUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
    .SetHandlerLifetime(TimeSpan.FromMinutes(10));
});

host.Run();
