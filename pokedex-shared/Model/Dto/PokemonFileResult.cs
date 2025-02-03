namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonFileResult(
    string FileName,
    string ContentType,
    MemoryStream File
);