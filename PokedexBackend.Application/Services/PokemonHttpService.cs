using System.Net;
using System.Text.RegularExpressions;
using PokedexBackend.Domain.Models;
using PokedexBackend.Application.Dtos;
using PokedexBackend.Domain.Interfaces.Services;
using PokedexBackend.Application.Utils;
using PokedexBackend.Domain.Common;
using PokedexBackend.Domain.Enums;
using PokedexBackend.Domain.Interfaces;

namespace PokedexBackend.Application.Services;

public class PokemonHttpService : IPokemonHttpService
{
    private readonly HttpClient _httpClient;
   
    private readonly ICacheRepository _cacheRepository;

    public PokemonHttpService(HttpClient httpClient, ICacheRepository cacheRepository)
    {
        _httpClient = httpClient; 
        _cacheRepository = cacheRepository;
    }

    public async Task<OperationResult<Pokemon>> GetPokemonByName(string name)
    {
        var cachePokemon = await _cacheRepository.GetAsync(name);
        if (!string.IsNullOrEmpty(cachePokemon))
        {
            return OperationResult<Pokemon>.Success(JsonHelper.Deserialize<Pokemon>(cachePokemon));
        }
      
        var response = await _httpClient.GetAsync($"pokemon/{name}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return OperationResult<Pokemon>.Failure(EOperationResultCode.NotFound, $"Pokemon '{name}' not found");
        }
        else if (!response.IsSuccessStatusCode)
        {
            return OperationResult<Pokemon>.Failure(EOperationResultCode.ServerError, "Internal Error Server");
        }
      
        var jsonResponse = await response.Content.ReadAsStringAsync();

        var pokemon = JsonHelper.Deserialize<Pokemon>(jsonResponse);
      
        pokemon.UrlImage = PokemonImageHelper.GetImageUrl(jsonResponse);
        
        pokemon.Height = pokemon.Height / 10;
        pokemon.Weight = pokemon.Weight / 10;

        await _cacheRepository.SetAsync(pokemon.Name,JsonHelper.Serialize(pokemon));
      
        return OperationResult<Pokemon>.Success(pokemon);
    }
   
    public async Task<OperationResult<PokemonsContainsInNamePaginated>> GetPokemonContainsInName(string name,  int? pageNumber = null, int? pageSize = null, string? baseUrl = null)
    {
        var pokemonsGetAll =  await GetAllPokemons();
        if (pokemonsGetAll.Code == EOperationResultCode.ServerError)
        {
            return OperationResult<PokemonsContainsInNamePaginated>.Failure(EOperationResultCode.ServerError, $"Internal Server Error");
        }
        List<PokemonField> pokemons = pokemonsGetAll.Value!;

        List<Pokemon> containsInNamePokemons = new List<Pokemon>(); 
      
        foreach (PokemonField pokemonField in pokemons)
        {
            if (pokemonField.Name.Contains(name))
            {
                var pokemon =  await GetPokemonByName(pokemonField.Name);
                if (pokemon.Value != null)
                    containsInNamePokemons.Add(pokemon.Value); 
            } 
        }

        if (containsInNamePokemons.Count == 0)
        {
            return OperationResult<PokemonsContainsInNamePaginated>.Failure(EOperationResultCode.NotFound, $"No Pokémon found containing in the name '{name}'");
        }
        
        var pages  = PaginationHelper.CreatePageResult(containsInNamePokemons, pageNumber, pageSize, baseUrl); 
            
        return OperationResult<PokemonsContainsInNamePaginated>.Success(new PokemonsContainsInNamePaginated
        {
            ContainsInName =  name,
            Next = pages.Next,
            Previous = pages.Previous,
            CurrentListSize =  pages.ListCount,
            TotalListSize = containsInNamePokemons.Count,
            TotalPages = pages.TotalPages,
            Pokemons = pages.Pokemons
        });
    }

    public async Task<OperationResult<PokemonsContainsInNamePaginated>> GetPokemons(int? pageNumber = null, int? pageSize = null, string? baseUrl = null)
    {
        int? offset = pageNumber == 0 || pageNumber == null  ?  0 :  (pageNumber - 1) * pageSize  ;
        var response = await _httpClient.GetAsync($"pokemon?limit={pageSize}&offset={offset}");
        
        if (!response.IsSuccessStatusCode)
        {
            return OperationResult<PokemonsContainsInNamePaginated>.Failure(EOperationResultCode.ServerError, "Internal Server Error");
        } 
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var pokemonsList = JsonHelper.Deserialize<AllPokemonsDto>(jsonResponse);
        
        List<Pokemon> containsInNamePokemons = new List<Pokemon>(); 
      
        foreach (PokemonField pokemonField in pokemonsList.Results)
        {
            var pokemon =  await GetPokemonByName(pokemonField.Name);
            if (pokemon.Value != null)
                containsInNamePokemons.Add(pokemon.Value); 
        }
        
        var responseGetLastId = await _httpClient.GetAsync($"pokemon?limit=1000000");
        responseGetLastId.EnsureSuccessStatusCode();
        
        var GetLastIdJsonResponse = await responseGetLastId.Content.ReadAsStringAsync();
        var GetLastIdPokemonsList = JsonHelper.Deserialize<AllPokemonsDto>(GetLastIdJsonResponse);

        // string LastIdUrl = GetLastIdPokemonsList.Results.Last().Url;
        // var match = Regex.Match(LastIdUrl, @"pokemon/(\d+)/");
        // int lastId =  int.Parse(match.Groups[1].Value);
        
        int totalAPICount = GetLastIdPokemonsList.Count;
        
        var pages = PaginationHelper.CreatePageResult(containsInNamePokemons, pageNumber, pageSize, baseUrl, totalAPICount);
     
        return OperationResult<PokemonsContainsInNamePaginated>.Success(new PokemonsContainsInNamePaginated
        {
            Next = pages.Next,
            Previous = pages.Previous,
            CurrentListSize =  pages.ListCount,
            TotalListSize = totalAPICount,
            TotalPages = pages.TotalPages,
            Pokemons = pages.Pokemons
        });
    }

    private async Task<OperationResult<List<PokemonField>>> GetAllPokemons()
    {
        var listName = "pokemonsNames";

        var pokemonsNamesList = await _cacheRepository.GetListAsync<PokemonField>(listName); 

        if (pokemonsNamesList?.Count >= 1)
        {
            return OperationResult<List<PokemonField>>.Success(pokemonsNamesList);
        }
        
        var response = await _httpClient.GetAsync($"pokemon?limit=1000000");
        
        if (!response.IsSuccessStatusCode)
        {
            return OperationResult<List<PokemonField>>.Failure(EOperationResultCode.ServerError, "Internal Server Error");
        } 
      
        var jsonResponse = await response.Content.ReadAsStringAsync();
      
        var allPokemons = JsonHelper.Deserialize<AllPokemonsDto>(jsonResponse);
      
        await _cacheRepository.SetListAsync(listName,allPokemons.Results);
     
        return OperationResult<List<PokemonField>>.Success(allPokemons.Results);
    }

    public async Task<OperationResult<CategoryPaginated>> GetPokemonsByType(string type, int? pageNumber = null, int? pageSize = null, string? baseUrl = null)
    {
        var typeKey = $"{type}-type";

        if (!AvaliableTypes.IsValidType(type))
        {
            return OperationResult<CategoryPaginated>.Failure(EOperationResultCode.BadRequest, $"{type} is not a pokemon type");
        }

        var cacheResponsePokemonsType = await _cacheRepository.GetAsync(typeKey);

        if (!string.IsNullOrEmpty(cacheResponsePokemonsType))
        {
            var typePokemons = JsonHelper.Deserialize<TypePokemons>(cacheResponsePokemonsType);
            var pageResult = PaginationHelper.CreatePageResult(typePokemons.Pokemons, pageNumber, pageSize, baseUrl); 
            
            return OperationResult<CategoryPaginated>.Success(new CategoryPaginated
            {
                Id = typePokemons.Id,
                Name = typePokemons.Name,
                Previous = pageResult.Previous,
                Next = pageResult.Next,
                CurrentListSize = pageResult.ListCount,
                TotalListSize = typePokemons.ListCount,
                TotalPages = pageResult.TotalPages,
                Pokemons = pageResult.Pokemons,
            });
        }
        
        var response = await _httpClient.GetAsync($"type/{type}");
        
        if (!response.IsSuccessStatusCode)
        {
            return OperationResult<CategoryPaginated>.Failure(EOperationResultCode.ServerError, "Internal Error Server");
        } 
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
      
        var responsePokemonsType = JsonHelper.Deserialize<TypeDto>(jsonResponse);

        List<Pokemon> pokemonsByType = new List<Pokemon>();
      
        foreach (PokemonList pokemonList in responsePokemonsType.Pokemon)
        {
            PokemonField pokemonField = pokemonList.Pokemon;
            var pokemon =  await GetPokemonByName(pokemonField.Name);
            if (pokemon.Value != null)
                pokemonsByType.Add(pokemon.Value);
        }

        var pokemonsType = new TypePokemons
        {
            Id = responsePokemonsType.Id,
            Name = responsePokemonsType.Name,
            ListCount = pokemonsByType.Count,
            Pokemons = pokemonsByType
        };

        await _cacheRepository.SetAsync(typeKey, JsonHelper.Serialize(pokemonsType));
        
        var pages  = PaginationHelper.CreatePageResult(pokemonsType.Pokemons,pageNumber, pageSize, baseUrl); 
            
        return OperationResult<CategoryPaginated>.Success(new CategoryPaginated
        {
            Id = pokemonsType.Id,
            Name = pokemonsType.Name,
            Previous = pages.Previous,
            Next = pages.Next,
            CurrentListSize = pages.ListCount,
            TotalListSize = pokemonsType.ListCount,
            TotalPages = pages.TotalPages,
            Pokemons = pages.Pokemons,
        });
        
    }
    public async Task<OperationResult<CategoryPaginated>> GetPokemonsByHabitat(string habitat, int? pageNumber = null, int? pageSize = null, string? baseUrl = null)
    { 
        var habitatKey = $"{habitat}-habitat";
        if (!AvaliableHabitats.IsValidHabitat(habitat))
        {
            return OperationResult<CategoryPaginated>.Failure(EOperationResultCode.BadRequest, $"{habitat} is not a pokemon habitat");
        }
        var cacheResponsePokemonsHabitat = await _cacheRepository.GetAsync(habitatKey);

        if (!string.IsNullOrEmpty(cacheResponsePokemonsHabitat))
        {
            var habitatPokemons = JsonHelper.Deserialize<HabitatPokemons>(cacheResponsePokemonsHabitat);
            var pageResult = PaginationHelper.CreatePageResult(habitatPokemons.Pokemons, pageNumber, pageSize, baseUrl); 
            
            return OperationResult<CategoryPaginated>.Success(new CategoryPaginated
            {
                Id = habitatPokemons.Id,
                Name = habitatPokemons.Name,
                Previous = pageResult.Previous,
                Next = pageResult.Next,
                CurrentListSize = pageResult.ListCount,
                TotalListSize = habitatPokemons.ListCount,
                TotalPages =  pageResult.TotalPages, 
                Pokemons = pageResult.Pokemons,
            });
        }
        
        var response = await _httpClient.GetAsync($"pokemon-habitat/{habitat}");
        
        if (!response.IsSuccessStatusCode)
        {
            return OperationResult<CategoryPaginated>.Failure(EOperationResultCode.ServerError, "Internal Error Server");
        } 
      
        var jsonResponse = await response.Content.ReadAsStringAsync();
      
        var responsePokemonsHabitat = JsonHelper.Deserialize<HabitatDto>(jsonResponse);
        
        List<Pokemon> pokemonsByHabitat = new List<Pokemon>();

        foreach(PokemonField pokemonField in responsePokemonsHabitat.PokemonSpecies)
        {
            var pokemon =  await GetPokemonByName(pokemonField.Name);
            if (pokemon.Value != null)
                pokemonsByHabitat.Add(pokemon.Value);
        }
        
        var pokemonsHabitat = new HabitatPokemons
        {
            Id = responsePokemonsHabitat.Id,
            Name = responsePokemonsHabitat.Name,
            ListCount = pokemonsByHabitat.Count,
            Pokemons = pokemonsByHabitat,
        }; 
        
        await _cacheRepository.SetAsync(habitatKey, JsonHelper.Serialize(pokemonsHabitat));
        
        var pages  = PaginationHelper.CreatePageResult(pokemonsHabitat.Pokemons, pageNumber, pageSize, baseUrl); 
            
        return OperationResult<CategoryPaginated>.Success(new CategoryPaginated
        {
            Id = pokemonsHabitat.Id,
            Name = pokemonsHabitat.Name,
            Previous = pages.Previous,
            Next = pages.Next,
            CurrentListSize = pages.ListCount,
            TotalListSize = pokemonsHabitat.ListCount,
            TotalPages = pages.TotalPages,
            Pokemons = pages.Pokemons,
        });
    }

    public async Task<OperationResult<MultiplesFiltersPaginated>> GetPokemonsMultiplesFilters(string name, string type, string habitat, int? pageNumber = null, int? pageSize = null, string? baseUrl = null)
    {
        List<Pokemon>? pokemonsByName = new List<Pokemon>();
        CategoryPaginated? pokemonsByType = new CategoryPaginated();
        CategoryPaginated? habitatPokemons = new CategoryPaginated();

        if (!string.IsNullOrEmpty(name))
        {
            var requestNameResult = await GetPokemonContainsInName(name);
            pokemonsByName = requestNameResult.Value?.Pokemons;
        }

        if (!string.IsNullOrEmpty(type))
        {
            var requestTypeResult = await GetPokemonsByType(type);
            pokemonsByType = requestTypeResult.Value;
        }
        
        if (!string.IsNullOrEmpty(habitat))
        {
            var requestHabitatResult = await GetPokemonsByHabitat(habitat);
            habitatPokemons = requestHabitatResult.Value;

        }
        
        List<Pokemon> result = new List<Pokemon>();
        
        if (pokemonsByName != null &&  pokemonsByName.Any())
        {
            result = pokemonsByName;
        }

        if (pokemonsByType != null && pokemonsByType.Pokemons.Any())
        {
            result = result.Any() ? result.Intersect(pokemonsByType.Pokemons).ToList() : pokemonsByType.Pokemons.ToList();
        }

        if (habitatPokemons != null && habitatPokemons.Pokemons.Any())
        {
            result = result.Any() ? result.Intersect(habitatPokemons.Pokemons).ToList() : habitatPokemons.Pokemons.ToList();
        }
        
        string pattern = @"([?&]pageNumber=\d+)|([?&]pageSize=\d+)";
        
        string newBaseUrl = Regex.Replace(baseUrl, pattern, "");
        
        var pages  = PaginationHelper.CreatePageResult(result, pageNumber, pageSize, newBaseUrl); 

        return OperationResult<MultiplesFiltersPaginated>.Success(new MultiplesFiltersPaginated
        {
           ContainsInName = name,
           TypeName = type,
           TypeId = pokemonsByType != null ? pokemonsByType.Id : 0,
           HabitatName = habitat,
           HabitatId = habitatPokemons != null ?   habitatPokemons.Id : 0,
           Next = pages.Next,
           Previous = pages.Previous,
           CurrentListSize = pages.ListCount,
           TotalListSize = result.Count,
           TotalPages = pages.TotalPages,
           Pokemons = pages.Pokemons, 
        });
    }
}