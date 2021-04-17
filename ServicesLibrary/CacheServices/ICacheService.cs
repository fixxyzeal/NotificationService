using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary.CacheServices
{
    public interface ICacheService
    {
        Task<bool> Delete(string key);

        Task<T> Get<T>(string key);

        Task<bool> Set<T>(string key, T value, DateTime? expire);
    }
}