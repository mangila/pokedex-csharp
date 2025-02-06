using System.ComponentModel.DataAnnotations;
using pokedex_shared.Model.Domain;

namespace pokedex_shared.Common.Attribute;

public class ValidPokemonSpecialListAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is List<string> stringList)
        {
            if (stringList.Count > PokemonSpecial.ToArray().Length)
            {
                return new ValidationResult("to many pokemon special");
            }

            foreach (var element in stringList)
            {
                try
                {
                    PokemonSpecial.From(element);
                }
                catch (NotSupportedException e)
                {
                    return new ValidationResult(e.Message);
                }
            }

            return ValidationResult.Success;
        }

        return new ValidationResult("Invalid data type. Expected a list of strings.");
    }
}