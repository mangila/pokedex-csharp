namespace pokedex_shared.Http.Species;

public readonly record struct PokemonSpeciesApiResponse(
    int? base_happiness,
    int? capture_rate,
    Color color,
    Egg_groups[] egg_groups,
    Evolution_chain evolution_chain,
    object evolves_from_species,
    Flavor_text_entries[] flavor_text_entries,
    object[] form_descriptions,
    bool forms_switchable,
    int? gender_rate,
    Genera[] genera,
    Generation generation,
    Growth_rate growth_rate,
    Habitat? habitat,
    bool has_gender_differences,
    int? hatch_counter,
    int? id,
    bool is_baby,
    bool is_legendary,
    bool is_mythical,
    string name,
    Names[] names,
    int? order,
    Pal_park_encounters[] pal_park_encounters,
    Pokedex_numbers[] pokedex_numbers,
    Shape shape,
    Varieties[] varieties
);

public readonly record struct Color(
    string name,
    string url
);

public readonly record struct Egg_groups(
    string name,
    string url
);

public readonly record struct Evolution_chain(
    string url
);

public readonly record struct Flavor_text_entries(
    string flavor_text,
    Language language,
    Version version
);

public readonly record struct Language(
    string name,
    string url
);

public readonly record struct Version(
    string name,
    string url
);

public readonly record struct Genera(
    string genus,
    Language1 language
);

public readonly record struct Language1(
    string name,
    string url
);

public readonly record struct Generation(
    string name,
    string url
);

public readonly record struct Growth_rate(
    string name,
    string url
);

public readonly record struct Habitat(
    string name,
    string url
);

public readonly record struct Names(
    Language2 language,
    string name
);

public readonly record struct Language2(
    string name,
    string url
);

public readonly record struct Pal_park_encounters(
    Area area,
    int? base_score,
    int? rate
);

public readonly record struct Area(
    string name,
    string url
);

public readonly record struct Pokedex_numbers(
    int? entry_number,
    Pokedex pokedex
);

public readonly record struct Pokedex(
    string name,
    string url
);

public readonly record struct Shape(
    string name,
    string url
);

public readonly record struct Varieties(
    bool is_default,
    Pokemon pokemon
);

public readonly record struct Pokemon(
    string name,
    string url
);