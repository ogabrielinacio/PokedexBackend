using PokedexBackend.Domain.Common;
using PokedexBackend.Domain.Models;

namespace PokedexBackend.Domain.Interfaces.Services;

public interface IPokemonHttpService
{

    Task<OperationResult<Pokemon>> GetPokemonByName(string name);

    Task<OperationResult<PokemonsContainsInNamePaginated>> GetPokemonContainsInName(string name,  int? pageNumber = null, int? pageSize = null, string? baseUrl = null);
    
    Task<OperationResult<PokemonsContainsInNamePaginated>> GetPokemons(int? pageNumber = null, int? pageSize = null, string? baseUrl = null);
    
    Task<OperationResult<CategoryPaginated>> GetPokemonsByType(string type, int? pageNumber = null, int? pageSize = null, string? baseUrl = null);
    
    Task<OperationResult<CategoryPaginated>> GetPokemonsByHabitat(string habitat,  int? pageNumber = null, int? pageSize = null, string? baseUrl = null);
    
    Task<OperationResult<MultiplesFiltersPaginated>> GetPokemonsMultiplesFilters(string name, string type, string habitat, int? pageNumber = null, int? pageSize = null, string? baseUrl = null);
}
