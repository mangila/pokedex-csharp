using System.ComponentModel.DataAnnotations;

namespace pokedex_shared.Option;

public class PokeApiOption
{
    [Required] public required string Url { get; init; }
    [Required] public required string GetPokemonGenerationUri { get; init; }
}