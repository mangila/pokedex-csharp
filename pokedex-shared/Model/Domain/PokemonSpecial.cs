namespace pokedex_shared.Model.Domain;

/**
 * <summary>
 *  Pokémon Special: <br></br>
 *  - Legendary <br></br>
 *  - Baby <br></br>
 *  - Mythical <br></br>
 * </summary>
 */
public record struct PokemonSpecial
{
    public static readonly PokemonSpecial Legendary = new("legendary");
    public static readonly PokemonSpecial Baby = new("baby");
    public static readonly PokemonSpecial Mythical = new("mythical");

    public string Name { get; }
    private PokemonSpecial(string name) => Name = name;

    public static PokemonSpecial From(string value)
    {
        return value switch
        {
            "legendary" => Legendary,
            "baby" => Baby,
            "mythical" => Mythical,
            _ => throw new NotSupportedException("Unknown Pokémon special: " + value)
        };
    }

    public static PokemonSpecial[] ToArray()
    {
        return
        [
            Legendary,
            Baby,
            Mythical
        ];
    }
}