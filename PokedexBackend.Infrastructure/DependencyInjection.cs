using Microsoft.Extensions.DependencyInjection;
using PokedexBackend.Domain.Interfaces;
using PokedexBackend.Infrastructure.Helper;
using PokedexBackend.Infrastructure.Repository;

namespace PokedexBackend.Infrastructure;

public static class DependencyInjection
{
   public static IServiceCollection  AddInfrastructureDI(this IServiceCollection services)
   {
       services.AddScoped<ICacheRepository, CacheRepository>();
       services.AddStackExchangeRedisCache(redisOptions =>
       {
           redisOptions.Configuration = new CacheHelper().GetConnectionString();
       });
     return services;
   }
}