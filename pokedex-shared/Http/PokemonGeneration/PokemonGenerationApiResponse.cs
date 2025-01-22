namespace pokedex_shared.Http.PokemonGeneration;

public record PokemonGenerationApiResponse(
    object[] abilities,
    int id,
    Main_region main_region,
    Moves[] moves,
    string name,
    Names[] names,
    Pokemon_species[] pokemon_species,
    Types[] types,
    Version_groups[] version_groups
);

public record Main_region(
    string name,
    string url
);

public record Moves(
    string name,
    string url
);

public record Names(
    Language language,
    string name
);

public record Language(
    string name,
    string url
);

public record Pokemon_species(
    string name,
    string url
);

public record Types(
    string name,
    string url
);

public record Version_groups(
    string name,
    string url
);