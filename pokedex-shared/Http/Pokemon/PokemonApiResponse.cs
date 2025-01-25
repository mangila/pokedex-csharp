using System.Text.Json.Serialization;

namespace pokedex_shared.Http.Pokemon;

public readonly record struct PokemonApiResponse(
    Abilities[] abilities,
    int? base_experience,
    Cries cries,
    Forms[] forms,
    Game_indices[] game_indices,
    int height,
    object[] held_items,
    int id,
    bool is_default,
    string location_area_encounters,
    Moves[] moves,
    string name,
    int? order,
    object[] past_abilities,
    object[] past_types,
    Species species,
    Sprites sprites,
    Stats[] stats,
    Types[] types,
    int weight
);

public readonly record struct Abilities(
    Ability ability,
    bool is_hidden,
    int? slot
);

public readonly record struct Ability(
    string name,
    string url
);

public readonly record struct Cries(
    string? latest,
    string? legacy
);

public readonly record struct Forms(
    string name,
    string url
);

public readonly record struct Game_indices(
    int? game_index,
    Version version
);

public readonly record struct Version(
    string name,
    string url
);

public readonly record struct Moves(
    Move move,
    Version_group_details[] version_group_details
);

public readonly record struct Move(
    string name,
    string url
);

public readonly record struct Version_group_details(
    int? level_learned_at,
    Move_learn_method move_learn_method,
    Version_group version_group
);

public readonly record struct Move_learn_method(
    string name,
    string url
);

public readonly record struct Version_group(
    string name,
    string url
);

public readonly record struct Species(
    string name,
    string url
);

public readonly record struct Sprites(
    string back_default,
    object back_female,
    string back_shiny,
    object back_shiny_female,
    string? front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female,
    Other other,
    Versions versions
);

public readonly record struct Other(
    Dream_world dream_world,
    Home home,
    [property: JsonPropertyName("official-artwork")]
    Official_artwork official_artwork,
    Showdown showdown
);

public readonly record struct Dream_world(
    string front_default,
    object front_female
);

public readonly record struct Home(
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Official_artwork(
    string? front_default,
    string front_shiny
);

public readonly record struct Showdown(
    string back_default,
    object back_female,
    string back_shiny,
    object back_shiny_female,
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Versions(
    Generation_i generation_i,
    Generation_ii generation_ii,
    Generation_iii generation_iii,
    Generation_iv generation_iv,
    Generation_v generation_v,
    Generation_vi generation_vi,
    Generation_vii generation_vii,
    Generation_viii generation_viii
);

public readonly record struct Generation_i(
    Red_blue red_blue,
    Yellow yellow
);

public readonly record struct Red_blue(
    string back_default,
    string back_gray,
    string back_transparent,
    string front_default,
    string front_gray,
    string front_transparent
);

public readonly record struct Yellow(
    string back_default,
    string back_gray,
    string back_transparent,
    string front_default,
    string front_gray,
    string front_transparent
);

public readonly record struct Generation_ii(
    Crystal crystal,
    Gold gold,
    Silver silver
);

public readonly record struct Crystal(
    string back_default,
    string back_shiny,
    string back_shiny_transparent,
    string back_transparent,
    string front_default,
    string front_shiny,
    string front_shiny_transparent,
    string front_transparent
);

public readonly record struct Gold(
    string back_default,
    string back_shiny,
    string front_default,
    string front_shiny,
    string front_transparent
);

public readonly record struct Silver(
    string back_default,
    string back_shiny,
    string front_default,
    string front_shiny,
    string front_transparent
);

public readonly record struct Generation_iii(
    Emerald emerald,
    Firered_leafgreen firered_leafgreen,
    Ruby_sapphire ruby_sapphire
);

public readonly record struct Emerald(
    string front_default,
    string front_shiny
);

public readonly record struct Firered_leafgreen(
    string back_default,
    string back_shiny,
    string front_default,
    string front_shiny
);

public readonly record struct Ruby_sapphire(
    string back_default,
    string back_shiny,
    string front_default,
    string front_shiny
);

public readonly record struct Generation_iv(
    Diamond_pearl diamond_pearl,
    Heartgold_soulsilver heartgold_soulsilver,
    Platinum platinum
);

public readonly record struct Diamond_pearl(
    string back_default,
    object back_female,
    string back_shiny,
    object back_shiny_female,
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Heartgold_soulsilver(
    string back_default,
    object back_female,
    string back_shiny,
    object back_shiny_female,
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Platinum(
    string back_default,
    object back_female,
    string back_shiny,
    object back_shiny_female,
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Generation_v(
    Black_white black_white
);

public readonly record struct Black_white(
    Animated animated,
    string back_default,
    object back_female,
    string back_shiny,
    object back_shiny_female,
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Animated(
    string back_default,
    object back_female,
    string back_shiny,
    object back_shiny_female,
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Generation_vi(
    Omegaruby_alphasapphire omegaruby_alphasapphire,
    X_y x_y
);

public readonly record struct Omegaruby_alphasapphire(
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct X_y(
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Generation_vii(
    Icons icons,
    Ultra_sun_ultra_moon ultra_sun_ultra_moon
);

public readonly record struct Icons(
    string front_default,
    object front_female
);

public readonly record struct Ultra_sun_ultra_moon(
    string front_default,
    object front_female,
    string front_shiny,
    object front_shiny_female
);

public readonly record struct Generation_viii(
    Icons1 icons
);

public readonly record struct Icons1(
    string front_default,
    object front_female
);

public readonly record struct Stats(
    int? base_stat,
    int? effort,
    Stat stat
);

public readonly record struct Stat(
    string name,
    string url
);

public readonly record struct Types(
    int? slot,
    Type type
);

public readonly record struct Type(
    string name,
    string url
);