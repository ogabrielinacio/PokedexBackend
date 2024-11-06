namespace PokedexBackend.Domain.Common;

public class AvaliableTypes
{
    public const string Normal = "normal";
    public const string Fighting = "fighting";
    public const string Flying = "flying";
    public const string Poison = "poison";
    public const string Ground = "ground";
    public const string Rock = "rock";
    public const string Bug = "bug";
    public const string Ghost = "ghost";
    public const string Steel = "steel";
    public const string Fire = "fire";
    public const string Water = "water";
    public const string Grass = "grass";
    public const string Electric = "electric";
    public const string Psychic = "psychic";
    public const string Ice = "ice";
    public const string Dragon = "dragon";
    public const string Dark = "dark";
    public const string Fairy = "fairy";
    public const string Stellar = "stellar";
    public const string Unknown = "unknown";
    public const string Shadow = "shadow";

    public static readonly HashSet<string> ValidTypes = new HashSet<string>
    {
        Normal, Fighting, Flying, Poison, Ground, Rock, Bug, Ghost, Steel, Fire, Water, Grass, Electric,
        Psychic, Ice, Dragon, Dark, Fairy, Stellar, Unknown, Shadow
    };

    public static bool IsValidType(string input)
    {
        return ValidTypes.Contains(input.ToLower());
    } 
}