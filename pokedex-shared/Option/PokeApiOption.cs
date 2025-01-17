using System.ComponentModel.DataAnnotations;

namespace pokedex_poller.Config;

public class PokeApiOption
{
    [Required] public required string Url { get; init; }
    [Required] public required string GetPokemonUri { get; init; }
}