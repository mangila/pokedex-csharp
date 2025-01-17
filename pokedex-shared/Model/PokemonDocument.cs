using System.ComponentModel.DataAnnotations;

namespace pokedex_shared.Model;

public class PokemonDocument
{
    [Required]
    [Key]
    [Range(1, 151, ErrorMessage = "Id can only be Gen I pokemon")]
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be more than 100 chars")]
    public required string Name { get; set; }
}

public static partial class Extensions
{
    public static PokemonDto ToDto(this PokemonDocument document)
    {
        return new PokemonDto(
            Id: document.Id,
            Name: document.Name
        );
    }

    private static void Validate(this PokemonDocument document)
    {
        var context = new ValidationContext(document, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(document, context, results, validateAllProperties: true);
        if (results.Count != 0)
        {
            throw new ValidationException(string.Join(", ", results));
        }
    }
}