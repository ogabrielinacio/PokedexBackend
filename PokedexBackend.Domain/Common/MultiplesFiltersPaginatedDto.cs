using PokedexBackend.Domain.Models;

namespace PokedexBackend.Domain.Common;

public class MultiplesFiltersPaginated
{
    public string ContainsInName { get; set; } = string.Empty;
    
    public string TypeName { get; set; } = string.Empty; 
    
    public int TypeId { get; set; }
    
    public string HabitatName { get; set; } = string.Empty; 
    
    public int HabitatId { get; set; }
     
    public string Next { get; set; } = string.Empty;
    
    public string Previous { get; set; } = string.Empty; 
    
    public int CurrentListSize { get; set; } 
    
    public int TotalListSize { get; set; } 
    
    public int TotalPages { get; set; } 
    
    public  List<Pokemon> Pokemons { get; set; }  = new List<Pokemon>();    
}