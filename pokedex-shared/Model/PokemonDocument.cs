using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pokedex_shared.Model;

public class PokemonDocument
{
    [Required] [BsonId] public ObjectId Id { get; init; }

    [Required] [BsonElement("pokemon_id")] public required string PokemonId { get; init; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be more than 100 chars")]
    [BsonElement("name")]
    public required string Name { get; init; }
}

public static partial class Extensions
{
    public static PokemonDto ToDto(this PokemonDocument document)
    {
        return new PokemonDto(
            PokemonId: document.PokemonId,
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