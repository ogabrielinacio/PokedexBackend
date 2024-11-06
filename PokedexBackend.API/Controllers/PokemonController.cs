using Microsoft.AspNetCore.Mvc;
using PokedexBackend.Domain.Common;
using PokedexBackend.Domain.Enums;
using PokedexBackend.Domain.Interfaces.Services;

public class PokemonController: ControllerBase
{
    private readonly IPokemonHttpService _pokemonService;
    
    public  PokemonController(IPokemonHttpService pokemonService)
    {
         _pokemonService = pokemonService;
    } 
    
    private IActionResult MapActionResult<T>(OperationResult<T> result)
    {
        return result.Code switch
        {
            EOperationResultCode.NotFound => NotFound(result),
            EOperationResultCode.BadRequest => BadRequest(result),
            EOperationResultCode.ServerError => StatusCode(500, result),
            _ => Ok(result)
        };
    }
  
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetPokemonByName([FromRoute] string name)
    {
        return MapActionResult(await _pokemonService.GetPokemonByName(name) );
    }
    
    [HttpGet("contains-in-name")]
    public async Task<IActionResult> GetPokemonByContainsInName([FromQuery] string name, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return MapActionResult(await _pokemonService.GetPokemonContainsInName(name, pageNumber, pageSize, $"contains-in-name?name={name}"));
    }
    [HttpGet("pokemons")]
    public async Task<IActionResult> GetAllPokemons( [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return MapActionResult( await _pokemonService.GetPokemons(pageNumber, pageSize, "pokemons"));
    }
    
    [HttpGet("type/{type}")]
    public async Task<IActionResult> GetPokemonsByType([FromRoute] string type, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return  MapActionResult( await _pokemonService.GetPokemonsByType(type, pageNumber, pageSize, $"type/{type}"));
    } 
    
    [HttpGet("habitat/{habitat}")]
    public async Task<IActionResult> GetPokemonsByHabitat([FromRoute] string habitat,  [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return MapActionResult(await _pokemonService.GetPokemonsByHabitat(habitat, pageNumber, pageSize, $"habitat/{habitat}"));
    } 
    
    [HttpGet("multiples-filter")]
    public async Task<IActionResult> MultiplesFilter([FromQuery] string name, [FromQuery]string type, [FromQuery]string habitat,  [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        return MapActionResult(await _pokemonService.GetPokemonsMultiplesFilters(name, type, habitat, pageNumber, pageSize, $"multiples-filter{Request.QueryString.Value}"));
    } 
    
        
}