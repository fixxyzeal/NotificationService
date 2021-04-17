using LazyCache;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary.CacheServices
{
    public class LazyCacheService : ICacheService
    {
        private readonly IAppCache _cache;

        public LazyCacheService(IAppCache cache)
        {
            _cache = cache;
        }

        public async Task<T> Get<T>(string key)
        {
            string result = await _cache.GetAsync<string>(key).ConfigureAwait(false);
            if (string.IsNullOrEmpty(result))
            {
                result = string.Empty;
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<bool> Set<T>(string key, T value, DateTime? expire)
        {
            await Task.Run(() => _cache.Add<string>(key, JsonConvert.SerializeObject(value), expire.Value)).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> Delete(string key)
        {
            await Task.Run(() => _cache.Remove(key)).ConfigureAwait(false);
            return true;
        }
    }
}