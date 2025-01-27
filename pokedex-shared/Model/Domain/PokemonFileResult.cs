namespace pokedex_shared.Model.Domain;

public readonly record struct PokemonFileResult(
    string FileName,
    string ContentType,
    MemoryStream File
);