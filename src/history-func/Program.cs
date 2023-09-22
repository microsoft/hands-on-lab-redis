using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using History.Api;

// TODO: load config from App Configuration: https://learn.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-azure-functions-csharp?tabs=isolated-process#reload-data-from-app-configuration

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IHistoryStoreService, HistoryStoreService>();
    })
    .Build();

host.Run();
