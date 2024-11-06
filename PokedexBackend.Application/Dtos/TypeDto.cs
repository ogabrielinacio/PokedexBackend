using PokedexBackend.Domain.Models;

namespace PokedexBackend.Application.Dtos;

public class TypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public  List<PokemonList> Pokemon { get; set; }  = new List<PokemonList>();
}

public class PokemonList {
    public PokemonField Pokemon { get; set; } = new PokemonField();
    public int Slot { get; set; }
}