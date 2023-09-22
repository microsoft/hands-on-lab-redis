using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace History.Api
{
    public class GetBrowsingHistory
    {
        private readonly ILogger _logger;
        private readonly IHistoryStoreService _historyStoreService;
        private readonly string userIdHeader = "X-USER-ID";

        public GetBrowsingHistory(ILoggerFactory loggerFactory, IHistoryStoreService historyStoreService)
        {
            _logger = loggerFactory.CreateLogger<GetBrowsingHistory>();
            _historyStoreService = historyStoreService;
        }

        [Function(nameof(GetBrowsingHistory))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "history")] HttpRequestData req)
        {
            if (!req.Headers.Contains(userIdHeader)) {
                var errorResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
                errorResponse.WriteString("Missing USER ID");
                return errorResponse;
            }

            string userId = req.Headers.GetValues(userIdHeader).First();
            _logger.LogInformation("Retrieving user {userId} history", userId);

            IEnumerable<ProductView> productViews = await _historyStoreService.GetHistory(userId);
            string historyJson = JsonSerializer.Serialize<IEnumerable<ProductView>>(productViews);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(historyJson);

            return response;
        }
    }
}
