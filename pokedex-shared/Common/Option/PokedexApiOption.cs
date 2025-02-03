using System.ComponentModel.DataAnnotations;

namespace pokedex_shared.Common.Option;

public class PokedexApiOption
{
    [Required] public required string Url { get; init; }
    [Required] public required string GetFileUri { get; init; }
}