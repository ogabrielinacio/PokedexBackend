namespace PokedexBackend.Domain.Models;

public class TypePokemons
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public int ListCount { get; set; } 
   
    public  List<Pokemon> Pokemons { get; set; }  = new List<Pokemon>(); 
}