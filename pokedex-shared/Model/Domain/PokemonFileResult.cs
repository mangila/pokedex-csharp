namespace pokedex_shared.Model.Domain;

public record PokemonFileResult(
    string FileName,
    string ContentType,
    MemoryStream File
);