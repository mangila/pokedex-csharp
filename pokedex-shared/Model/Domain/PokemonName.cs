using System.ComponentModel.DataAnnotations;
using pokedex_shared.Common;

namespace pokedex_shared.Model.Domain;

public readonly record struct PokemonName
{
    [Required]
    [StringLength(100, ErrorMessage = "length cannot be over 100")]
    [RegularExpression(@"^[A-Za-z-0-9]+$", ErrorMessage = "not matching A-Z or a-z")]
    public string Value { get; }

    public PokemonName(string value)
    {
        Value = value;
        this.ValidateValueType();
    }
}