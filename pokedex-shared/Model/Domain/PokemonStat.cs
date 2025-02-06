namespace pokedex_shared.Model.Domain;

/**
 * <summary>
 *  Pokémon Base Stats: <br></br>
 *  - HP <br></br>
 *  - Attack <br></br>
 *  - Defense <br></br>
 *  - Special Attack <br></br>
 *  - Special Defense <br></br>
 *  - Speed <br></br>
 * </summary>
 */
public record struct PokemonStat
{
    private static readonly PokemonStat Hp = new("hp");
    private static readonly PokemonStat Attack = new("attack");
    private static readonly PokemonStat Defense = new("defense");
    private static readonly PokemonStat SpecialAttack = new("special-attack");
    private static readonly PokemonStat SpecialDefense = new("special-defense");
    private static readonly PokemonStat Speed = new("speed");
    private static readonly PokemonStat Total = new("total");

    public string Name { get; }
    public int Value { get; set; }
    private PokemonStat(string name) => Name = name;

    public static PokemonStat From(string value)
    {
        return value switch
        {
            "hp" => Hp,
            "attack" => Attack,
            "defense" => Defense,
            "special-attack" => SpecialAttack,
            "special-defense" => SpecialDefense,
            "speed" => Speed,
            "total" => Total,
            _ => throw new NotSupportedException("Unknown Pokémon stat name: " + value)
        };
    }

    public static PokemonStat[] ToArray()
    {
        return
        [
            Hp,
            Attack,
            Defense,
            SpecialAttack,
            SpecialDefense,
            Speed
        ];
    }
}