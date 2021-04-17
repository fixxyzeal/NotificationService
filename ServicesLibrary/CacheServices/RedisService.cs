using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary.CacheServices
{
    public class RedisService : ICacheService
    {
        private readonly IDatabase db;

        public RedisService()
        {
            ConnectionMultiplexer muxer = ConnectionMultiplexer
                      .Connect($"{Environment.GetEnvironmentVariable("REDIS_HOST")}" +
                      $",password={Environment.GetEnvironmentVariable("REDIS_PASS")}" +
                      ",connectTimeout=3600000,connectRetry=5");
            db = muxer.GetDatabase();
        }

        public async Task<T> Get<T>(string key)
        {
            var result = await db.StringGetAsync(key).ConfigureAwait(false);
            if (string.IsNullOrEmpty(result))
            {
                result = string.Empty;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<bool> Set<T>(string key, T value, DateTime? expire)
        {
            await db.StringSetAsync(key, JsonConvert.SerializeObject(value)).ConfigureAwait(false);

            return await db.KeyExpireAsync(key, expire).ConfigureAwait(false);
        }

        public async Task<bool> Delete(string key)
        {
            return await db.KeyDeleteAsync(key).ConfigureAwait(false);
        }
    }
}