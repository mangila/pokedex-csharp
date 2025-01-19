using System.ComponentModel.DataAnnotations;

namespace pokedex_shared.Model;

public record PokemonName(
    [Required]
    [StringLength(100, ErrorMessage = "length cannot be over 100")]
    string name
);