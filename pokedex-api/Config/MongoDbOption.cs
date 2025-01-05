using System.ComponentModel.DataAnnotations;

namespace pokedex_api.Config;

public class MongoDbOption
{
    [Required] public required string ConnectionString { get; set; }
    [Required] public required string Database { get; set; }
    [Required] public required string Collection { get; set; }
}