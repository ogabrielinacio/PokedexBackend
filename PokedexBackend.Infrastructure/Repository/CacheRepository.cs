using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using PokedexBackend.Domain.Interfaces;

namespace PokedexBackend.Infrastructure.Repository;

public class CacheRepository: ICacheRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _distributedCacheEntryOptions;

    public CacheRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache; 
        _distributedCacheEntryOptions = new DistributedCacheEntryOptions
        {
        };
    }
        
    public async Task SetAsync(string key, string value, TimeSpan? expiration = null)
    {
        await _distributedCache.SetStringAsync(key, value, _distributedCacheEntryOptions);
    }

    public async Task<string> GetAsync(string key)
    {
        return await _distributedCache.GetStringAsync(key);
    }

    public async Task SetListAsync<T>(string key, IEnumerable<T> values, TimeSpan? expiration = null)
    {
        var jsonList = JsonSerializer.Serialize(values);
        await _distributedCache.SetStringAsync(key, jsonList, _distributedCacheEntryOptions);
    }

    public async Task<List<T>> GetListAsync<T>(string key)
    {
        var list =  await _distributedCache.GetStringAsync(key);
        
        return list == null ? null : JsonSerializer.Deserialize<List<T>>(list);
    }
    
    
}