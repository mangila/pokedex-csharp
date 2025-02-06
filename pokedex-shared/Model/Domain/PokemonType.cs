namespace pokedex_shared.Model.Domain;

/**
 * <summary>
 *  Pokémon Types: <br></br>
 *  - Normal <br></br>
 *  - Fighting <br></br>
 *  - Flying <br></br>
 *  - Poison <br></br>
 *  - Ground <br></br>
 *  - Rock <br></br>
 *  - Bug <br></br>
 *  - Ghost <br></br>
 *  - Steel <br></br>
 *  - Fire <br></br>
 *  - Water <br></br>
 *  - Grass <br></br>
 *  - Electric <br></br>
 *  - Psychic <br></br>
 *  - Ice <br></br>
 *  - Dragon <br></br>
 *  - Dark <br></br>
 *  - Fairy <br></br>
 * </summary>
 */
public readonly record struct PokemonType
{
    private static readonly PokemonType Normal = new("normal");
    private static readonly PokemonType Fighting = new("fighting");
    private static readonly PokemonType Flying = new("flying");
    private static readonly PokemonType Poison = new("poison");
    private static readonly PokemonType Ground = new("ground");
    private static readonly PokemonType Rock = new("rock");
    private static readonly PokemonType Bug = new("bug");
    private static readonly PokemonType Ghost = new("ghost");
    private static readonly PokemonType Steel = new("steel");
    private static readonly PokemonType Fire = new("fire");
    private static readonly PokemonType Water = new("water");
    private static readonly PokemonType Grass = new("grass");
    private static readonly PokemonType Electric = new("electric");
    private static readonly PokemonType Psychic = new("psychic");
    private static readonly PokemonType Ice = new("ice");
    private static readonly PokemonType Dragon = new("dragon");
    private static readonly PokemonType Dark = new("dark");
    private static readonly PokemonType Fairy = new("fairy");

    public string Value { get; }
    private PokemonType(string value) => Value = value;

    public static PokemonType From(string value)
    {
        return value switch
        {
            "normal" => Normal,
            "fighting" => Fighting,
            "flying" => Flying,
            "poison" => Poison,
            "ground" => Ground,
            "rock" => Rock,
            "bug" => Bug,
            "ghost" => Ghost,
            "steel" => Steel,
            "fire" => Fire,
            "water" => Water,
            "grass" => Grass,
            "electric" => Electric,
            "psychic" => Psychic,
            "ice" => Ice,
            "dragon" => Dragon,
            "dark" => Dark,
            "fairy" => Fairy,
            _ => throw new NotSupportedException("Unknown Pokémon type: " + value)
        };
    }

    public static PokemonType[] ToArray()
    {
        return
        [
            Normal,
            Fighting,
            Flying,
            Poison,
            Ground,
            Rock,
            Bug,
            Ghost,
            Steel,
            Fire,
            Water,
            Grass,
            Electric,
            Psychic,
            Ice,
            Dragon,
            Dark,
            Fairy
        ];
    }
}