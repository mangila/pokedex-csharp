namespace pokedex_shared.Model.Domain;

public readonly record struct PokemonMediaEntry(
    string FileName,
    string ContentType,
    byte[] File
);