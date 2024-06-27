using System;
using System.Text.Json;
using StackExchange.Redis;

namespace History.Api
{
    public interface IHistoryStoreService {
        Task<IEnumerable<ProductView>> GetHistory(string userId);
        Task AddToHistory(string userId, ProductView productView);
    }

    public class HistoryStoreService : IHistoryStoreService
    { 
        private readonly IDatabase _database;
        private readonly int _maxListSize = 10;
        private readonly string redisConnectionString = Environment.GetEnvironmentVariable("AZURE_REDIS_CONNECTION_STRING");

        public HistoryStoreService()
        {
            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString, AzureCacheForRedis.ConfigureForAzure);
            _database = connectionMultiplexer.GetDatabase();
        }

        private string UserHistoryKey(string userId) => $"history:{userId}";

        public async Task<IEnumerable<ProductView>> GetHistory(string userId)
        {
            string key = UserHistoryKey(userId);
            var list = await _database.ListRangeAsync(key, 0, -1);

            if (list == null) {
                return Enumerable.Empty<ProductView>();
            }

            var productViews = list.Select(item => JsonSerializer.Deserialize<ProductView>(item.ToString()));
            return productViews;
        }

        public async Task AddToHistory(string userId, ProductView productView)
        {
            string key = UserHistoryKey(userId);
            string productViewJson = JsonSerializer.Serialize<ProductView>(productView);

            await _database.ListLeftPushAsync(key, productViewJson);
            await _database.ListTrimAsync(key, 0, _maxListSize - 1);
        }
    }
}