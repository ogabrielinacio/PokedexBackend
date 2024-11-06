using PokedexBackend.Domain.Models;

namespace PokedexBackend.Application.Dtos;

public class PokemonsPaginatedDto
{
    public string Next { get; set; } = string.Empty;
    
    public string Previous { get; set; } = string.Empty; 
    
    public int ListCount { get; set; } 
    
    public int TotalPages { get; set; } 
    
    public  List<Pokemon> Pokemons { get; set; }  = new List<Pokemon>();   
}