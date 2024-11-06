namespace PokedexBackend.API;
using PokedexBackend.Application;

public static class DependencyInjection
{
   public static IServiceCollection AddAPIDI(this IServiceCollection services)
   {
       services.AddApplicationDI();
       return services;
   }
}