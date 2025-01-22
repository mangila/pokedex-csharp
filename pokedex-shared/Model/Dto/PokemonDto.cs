using System.Text.Json.Serialization;
using MongoDB.Bson;
using pokedex_shared.Extension;
using pokedex_shared.Model.Document;

namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonDto(
    [property: JsonPropertyName("pokemon_id")]
    string PokemonId,
    [property: JsonPropertyName("name")] string Name
);

public static class Extensions
{
    public static PokemonDocument ToDocument(this PokemonDto dto)
    {
        var document = new PokemonDocument
        {
            PokemonId = dto.PokemonId,
            Name = dto.Name,
            Types = new List<PokemonType>(),
            AudioId = new ObjectId(),
            SpriteId = new ObjectId()
        };
        document.Validate();
        return document;
    }
}