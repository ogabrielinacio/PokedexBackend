using System.Text.Json;

namespace PokedexBackend.Application.Utils;

public class JsonHelper
{
    
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy =  JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };
    public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _options);
    public static string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, _options);
}