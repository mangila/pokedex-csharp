using System.ComponentModel.DataAnnotations;
using pokedex_shared.Extension;

namespace pokedex_shared.Model.Domain;

public class PokemonName
{
    [Required]
    [StringLength(100, ErrorMessage = "length cannot be over 100")]
    [RegularExpression("[A-Za-z]+", ErrorMessage = "not matching A-Z or a-z")]
    public string Value { get; }

    public PokemonName(string value)
    {
        Value = value;
        this.Validate();
    }
}