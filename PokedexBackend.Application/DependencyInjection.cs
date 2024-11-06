using Microsoft.Extensions.DependencyInjection;
using PokedexBackend.Application.Services;
using PokedexBackend.Domain.Interfaces.Services;
using PokedexBackend.Infrastructure;

namespace PokedexBackend.Application;

public static class DependencyInjection
{
   public static IServiceCollection  AddApplicationDI(this IServiceCollection services)
   { 
       services.AddInfrastructureDI();
       services.AddHttpClient<IPokemonHttpService, PokemonHttpService>(client =>
       {
            client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
       });
     return services;
   }
}