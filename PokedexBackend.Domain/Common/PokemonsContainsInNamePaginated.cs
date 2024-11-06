using PokedexBackend.Domain.Models;

namespace PokedexBackend.Domain.Common;

public class PokemonsContainsInNamePaginated
{
    public string ContainsInName { get; set; } = string.Empty;
    
    public string Next { get; set; } = string.Empty;
    
    public string Previous { get; set; } = string.Empty; 
    
    public int CurrentListSize { get; set; } 
    
    public int TotalListSize { get; set; } 
    
    public int TotalPages { get; set; } 
    
    public  List<Pokemon> Pokemons { get; set; }  = new List<Pokemon>();   
}