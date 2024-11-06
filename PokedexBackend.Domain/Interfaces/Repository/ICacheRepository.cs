namespace PokedexBackend.Domain.Interfaces;

public interface ICacheRepository
{
  Task SetAsync(string key, string value, TimeSpan? expiration = null);
  Task<string> GetAsync(string key); 
  
  Task SetListAsync<T>(string key, IEnumerable<T> values, TimeSpan? expiration = null);
  
  Task<List<T>> GetListAsync<T>(string key);
}