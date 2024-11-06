using PokedexBackend.Domain.Models;

namespace  PokedexBackend.Application.Dtos;

public class AllPokemonsDto
{
    public int Count { get; set; }
    public string Next { get; set; } = string.Empty;
    public string Previous { get; set; } = string.Empty;
    public List<PokemonField> Results { get; set; } = new List<PokemonField>();
}