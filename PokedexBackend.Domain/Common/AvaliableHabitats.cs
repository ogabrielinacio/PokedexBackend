namespace PokedexBackend.Domain.Common;

public class AvaliableHabitats
{
    public const string Cave = "cave";
    public const string Forest = "forest";
    public const string Grassland = "grassland";
    public const string Mountain = "mountain";
    public const string Rare = "rare";
    public const string RoughTerrain = "rough-terrain";
    public const string Sea = "sea";
    public const string Urban = "urban";
    public const string WatersEdge = "waters-edge";

    public static readonly HashSet<string> ValidHabitats = new HashSet<string>
    {
        Cave, Forest, Grassland, Mountain, Rare, RoughTerrain, Sea, Urban, WatersEdge
    };

    public static bool IsValidHabitat(string input)
    {
        return ValidHabitats.Contains(input.ToLower());
    } 
}