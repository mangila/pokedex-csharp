using System.ComponentModel.DataAnnotations;
using pokedex_shared.Model.Domain;

namespace pokedex_shared.Common.Attribute;

public class ValidPokemonTypeListAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is List<string> stringList)
        {
            if (stringList.Count > PokemonType.ToArray().Length)
            {
                return new ValidationResult("to many pokemon types");
            }

            foreach (var element in stringList)
            {
                try
                {
                    PokemonType.From(element);
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