using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Caching
{
    public class DistributedCacheService : IDistributedCacheService
    {

        private readonly IDistributedCache distributedCache;

        public DistributedCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task AddOrUpdateCacheAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            await this.distributedCache.SetAsync(key, Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(value)), cancellationToken).ConfigureAwait(false);
        }

        public async Task<T> GetCacheAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var obj = await this.distributedCache.GetAsync(key, cancellationToken).ConfigureAwait(false);
            return obj != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(obj)) : default;
        }

        public async Task RemoveCacheAsync(string key, CancellationToken cancellationToken = default)
        {
            await this.distributedCache.RemoveAsync(key, cancellationToken).ConfigureAwait(false);
        }
    }
}
