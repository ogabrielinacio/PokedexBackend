namespace  PokedexBackend.Domain.Models;

public class Pokemon : IEquatable<Pokemon>
{
    public string Name { get; set; } = string.Empty;
    public string UrlImage { get; set; } = string.Empty;
    public int Id { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public List<AbilityField> Abilities { get; set; } =  new List<AbilityField>();
    public List<TypeField>  Types { get; set; } =  new List<TypeField>();
    public List<StatField> Stats {get; set; } = new List<StatField>();
    
    public bool Equals(Pokemon other)
    {
        if (other == null) return false;
        return this.Id == other.Id; 
    }

    public override bool Equals(object obj)
    {
        var other = obj as Pokemon;
        if (other == null) return false;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        return Id;
    }
    
}

public class StatInfo
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class StatField
{
    public int BaseStat { get; set; }
    public int Effort { get; set; }
    public StatInfo Stat { get; set; } =  new StatInfo();
}

public class TypeInfo
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class TypeField
{
    public int Slot { get; set; }
    public TypeInfo Type { get; set; } = new TypeInfo();
}

public class AbilityInfo
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class AbilityField
{
    public AbilityInfo Ability { get; set; } = new AbilityInfo();
    public bool IsHidden { get; set; }
    public int Slot { get; set; }
}
