using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Caching
{
    public interface IDistributedCacheService
    {
        Task AddOrUpdateCacheAsync<T>(string key, T value, CancellationToken cancellationToken = default);

        Task<T> GetCacheAsync<T>(string key, CancellationToken cancellationToken = default);

        Task RemoveCacheAsync(string key, CancellationToken cancellationToken = default);
    }
}
