using System.Text.Json;

namespace PokedexBackend.Application.Utils;

 public static class PokemonImageHelper
{

    public static string GetImageUrl(string jsonResponse)
    {
        using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
        {
            var sprites = doc.RootElement.GetProperty("sprites");
            
            if (sprites.TryGetProperty("front_default", out var frontDefault) && frontDefault.ValueKind != JsonValueKind.Null)
            {
                return frontDefault.GetString();
            }

            if (sprites.TryGetProperty("home", out var home) && home.TryGetProperty("front_default", out var homeFrontDefault) && homeFrontDefault.ValueKind != JsonValueKind.Null)
            {
                return homeFrontDefault.GetString();
            }

            return null;
        }
    }
}