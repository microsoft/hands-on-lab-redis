using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker.Extensions.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace Func.RedisCache.Products;

class Program
{
    static void Main(string[] args)
    {
        string catalogApiUrl = Environment.GetEnvironmentVariable("CATALOG_API_URL") ?? "";

        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(services =>
            {
                services.AddHttpClient(
                    Const.CATALOG_API_CLIENT,
                    client =>
                    {
                        client.BaseAddress = new Uri(catalogApiUrl);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    }
                )
                .SetHandlerLifetime(TimeSpan.FromMinutes(10));
            })

            .Build();

        host.Run();
    }
}
