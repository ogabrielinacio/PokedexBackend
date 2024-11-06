using PokedexBackend.Domain.Models;

namespace  PokedexBackend.Application.Dtos;

public class HabitatDto
{
   public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
   public List<PokemonField>  PokemonSpecies { get; set; } = new List<PokemonField>();
}