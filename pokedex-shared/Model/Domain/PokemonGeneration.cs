namespace pokedex_shared.Model.Domain;

/**
 * <summary>
 *  Red, Blue, Yellow (Gen 1) <br></br>
 *  Gold, Silver, Crystal (Gen 2) <br></br>
 *  Ruby, Sapphire, Emerald (Gen 3) <br></br>
 *  Diamond, Pearl, Platinum (Gen 4) <br></br>
 *  Black, White, Black 2, White 2 (Gen 5) <br></br>
 *  X, Y (Gen 6) <br></br>
 *  Sun, Moon, Ultra Sun, Ultra Moon (Gen 7) <br></br>
 *  Sword, Shield (Gen 8) <br></br>
 *  Scarlet, Violet (Gen 9) <br></br>
 * </summary>
 */
public readonly record struct PokemonGeneration
{
    private static readonly PokemonGeneration GenerationI = new("generation-i");
    private static readonly PokemonGeneration GenerationII = new("generation-ii");
    private static readonly PokemonGeneration GenerationIII = new("generation-iii");
    private static readonly PokemonGeneration GenerationIV = new("generation-iv");
    private static readonly PokemonGeneration GenerationV = new("generation-v");
    private static readonly PokemonGeneration GenerationVI = new("generation-vi");
    private static readonly PokemonGeneration GenerationVII = new("generation-vii");
    private static readonly PokemonGeneration GenerationVIII = new("generation-viii");
    private static readonly PokemonGeneration GenerationIX = new("generation-ix");
    public string Value { get; }
    private PokemonGeneration(string value) => Value = value;

    public static PokemonGeneration From(string value)
    {
        return value switch
        {
            "generation-i" => GenerationI,
            "generation-ii" => GenerationII,
            "generation-iii" => GenerationIII,
            "generation-iv" => GenerationIV,
            "generation-v" => GenerationV,
            "generation-vi" => GenerationVI,
            "generation-vii" => GenerationVII,
            "generation-viii" => GenerationVIII,
            "generation-ix" => GenerationIX,
            _ => throw new NotSupportedException("Unknown pokemon generation: " + value)
        };
    }

    public static PokemonGeneration[] ToArray()
    {
        return
        [
            GenerationI,
            GenerationII,
            GenerationIII,
            GenerationIV,
            GenerationV,
            GenerationVI,
            GenerationVII,
            GenerationVIII,
            GenerationIX
        ];
    }
}