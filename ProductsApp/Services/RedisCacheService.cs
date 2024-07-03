using Newtonsoft.Json;
using StackExchange.Redis;

namespace ProductsApp.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase _cacheDb;

        public RedisCacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }

        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            if (!value.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default(T);
        }

        public async Task SetCacheValueAsync(string key, object value)
        {
            await _cacheDb.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        public async Task RemoveCacheValueAsync(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }
    }
}
