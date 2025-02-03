using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace pokedex_shared.Model.Document.Embedded;

public readonly record struct PokemonVarietyDocument(
    [Required]
    [property: BsonElement("id")]
    int PokemonId,
    [Required]
    [property: BsonElement("name")]
    string Name
);