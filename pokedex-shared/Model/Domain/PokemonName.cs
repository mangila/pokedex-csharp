using System.ComponentModel.DataAnnotations;
using pokedex_shared.Extension;

namespace pokedex_shared.Model.Domain;

public readonly record struct PokemonName
{
    [Required]
    [StringLength(50, ErrorMessage = "length cannot be over 50")]
    [RegularExpression(@"^[A-Za-z-]+2?$", ErrorMessage = "not matching A-Z or a-z")]
    public string Value { get; }

    public PokemonName(string value)
    {
        Value = value;
        this.Validate();
    }
}