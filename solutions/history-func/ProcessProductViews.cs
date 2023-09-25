using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace History.Api
{
    public class ProductViewStreamEvent {
        public string Id { get; set; }

        public Dictionary<string,string> Values { get; set; }
    }

    public class ProcessProductViews
    {
        private readonly ILogger _logger;
        private readonly IHistoryStoreService _historyStoreService;

        public ProcessProductViews(ILoggerFactory loggerFactory, IHistoryStoreService historyStoreService)
        {
            _logger = loggerFactory.CreateLogger<ProcessProductViews>();
            _historyStoreService = historyStoreService;
        }

        [Function(nameof(ProcessProductViews))]
        public async Task StreamTrigger(
            [RedisStreamTrigger("AZURE_REDIS_CONNECTION_STRING", "%PRODUCT_VIEWS_STREAM_NAME%")] string entry
        )
        {
            _logger.LogInformation("Processing a new stream entry: {entry}", entry);

            var eventData = JsonSerializer.Deserialize<ProductViewStreamEvent>(entry);

            if (eventData == null) {
                return;
            }

            string userId = eventData.Values["userId"];

            ProductView productView = new ProductView {
                ProductId = eventData.Values["productId"],
                ProductTitle = eventData.Values["productTitle"],
                Date = eventData.Values["date"]
            };

            _logger.LogInformation("User {userId} viewed product {ProductId} on {date}", userId, productView.ProductId, productView.Date);

            await _historyStoreService.AddToHistory(userId, productView);
        }
    }
}
