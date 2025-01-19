using System.ComponentModel.DataAnnotations;

namespace pokedex_shared.Model;

public record PokemonId(
    [Required]
    [StringLength(5, ErrorMessage = "length cannot be over 5")]
    [RegularExpression("\\d+", ErrorMessage = "must be a number")]
    string id
);