using PokedexBackend.Application.Dtos;
using PokedexBackend.Domain.Models;

namespace PokedexBackend.Application.Utils;

public class PaginationHelper
{
    public static PokemonsPaginatedDto CreatePageResult(List<Pokemon> source, int? pageNumber = null, int? pageSize = null, string? baseUrl = null, int lastId = -1)
    {
        var listSize = lastId == -1  ? source.Count : lastId ;

        if ( pageSize == null || pageSize == 0 || pageSize >= listSize)
        {
            return new PokemonsPaginatedDto
            {
                ListCount = listSize,
                Pokemons = source,
                TotalPages = 1,
                Next = string.Empty,
                Previous = string.Empty, 
            };
        }
        
        pageNumber ??= 1; 
        baseUrl ??= string.Empty;
        
        var totalPages = (int)Math.Ceiling((double)listSize / pageSize.Value);

        var items = lastId != -1 ? source : source
            .Skip((pageNumber.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToList();

        var newBaseUrl = baseUrl.Contains("?") ? $"{baseUrl}&" : $"{baseUrl}?";

       return  new PokemonsPaginatedDto
        {
            ListCount = items.Count,
            Pokemons = items,
            TotalPages = totalPages,
            Next = pageNumber < totalPages ? $"{newBaseUrl}pageNumber={pageNumber + 1}&pageSize={pageSize}" : string.Empty,
            Previous = pageNumber > 1 ? $"{newBaseUrl}pageNumber={pageNumber - 1}&pageSize={pageSize}" : string.Empty, 
        };
    }
}