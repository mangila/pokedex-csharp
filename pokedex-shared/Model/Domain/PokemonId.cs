using System.ComponentModel.DataAnnotations;
using pokedex_shared.Extension;

namespace pokedex_shared.Model.Domain;

public class PokemonId
{
    [Required]
    [StringLength(5, ErrorMessage = "length cannot be over 5")]
    [RegularExpression("[\\d]+", ErrorMessage = "must be a number")]
    public string Value { get; }

    public PokemonId(string value)
    {
        Value = value;
        this.Validate();
    }

    public PokemonId(int value)
    {
        Value = value.ToString();
        this.Validate();
    }
}