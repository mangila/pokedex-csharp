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
public class PokemonGeneration
{
    public static readonly PokemonGeneration GenerationI = new("generation-i");
    public static readonly PokemonGeneration GenerationII = new("generation-ii");
    public static readonly PokemonGeneration GenerationIII = new("generation-iii");
    public static readonly PokemonGeneration GenerationIV = new("generation-iv");
    public static readonly PokemonGeneration GenerationV = new("generation-v");
    public static readonly PokemonGeneration GenerationVI = new("generation-vi");
    public static readonly PokemonGeneration GenerationVII = new("generation-vii");
    public static readonly PokemonGeneration GenerationVIII = new("generation-viii");
    public static readonly PokemonGeneration GenerationIX = new("generation-ix");
    public string Value { get; }

    private PokemonGeneration(string value) => Value = value;

    public override string ToString() => Value;
}