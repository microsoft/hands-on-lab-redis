using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using History.Api;
using Microsoft.AspNetCore.Server.Kestrel.Core;

// TODO: load config from App Configuration: https://learn.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-azure-functions-csharp?tabs=isolated-process#reload-data-from-app-configuration

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IHistoryStoreService, HistoryStoreService>();
        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
    })
    .Build();

host.Run();
