namespace PokedexBackend.Infrastructure.Helper;

public class CacheHelper
{
    public string GetConnectionString()
    {
        string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (env == "Production")
        {
            string redisUrl = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "";
            return redisUrl;
        }
        else
        {
            return  "localhost:6379";
        }
    } 
}