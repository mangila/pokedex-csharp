using System.Text.Json.Serialization;

namespace pokedex_shared.Integration.PokeApi.Response.Pokemon;

public readonly record struct Sprites(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_female")]
    string? BackFemale,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_female")]
    string? BackShinyFemale,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale,
    [property: JsonPropertyName("other")] Other Other,
    [property: JsonPropertyName("versions")]
    Versions Versions
);

public readonly record struct Other(
    [property: JsonPropertyName("dream_world")]
    DreamWorld DreamWorld,
    [property: JsonPropertyName("home")] Home Home,
    [property: JsonPropertyName("official-artwork")]
    OfficialArtwork OfficialArtwork,
    [property: JsonPropertyName("showdown")]
    Showdown Showdown
);

public readonly record struct DreamWorld(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale
);

public readonly record struct Home(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct OfficialArtwork(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny
);

public readonly record struct Showdown(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_female")]
    string? BackFemale,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_female")]
    string? BackShinyFemale,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct Versions(
    [property: JsonPropertyName("generation-i")]
    GenerationI GenerationI,
    [property: JsonPropertyName("generation-ii")]
    GenerationII GenerationII,
    [property: JsonPropertyName("generation-iii")]
    GenerationIII GenerationIII,
    [property: JsonPropertyName("generation-iv")]
    GenerationIV GenerationIV,
    [property: JsonPropertyName("generation-v")]
    GenerationV GenerationV,
    [property: JsonPropertyName("generation-vi")]
    GenerationVI GenerationVI,
    [property: JsonPropertyName("generation-vii")]
    GenerationVII GenerationVII,
    [property: JsonPropertyName("generation-viii")]
    GenerationVIII GenerationVIII
);

public readonly record struct GenerationI(
    [property: JsonPropertyName("red_blue")]
    RedBlue RedBlue,
    [property: JsonPropertyName("yellow")] Yellow Yellow
);

public readonly record struct RedBlue(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_gray")]
    string? BackGray,
    [property: JsonPropertyName("back_transparent")]
    string? BackTransparent,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_gray")]
    string? FrontGray,
    [property: JsonPropertyName("front_transparent")]
    string? FrontTransparent
);

public readonly record struct Yellow(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_gray")]
    string? BackGray,
    [property: JsonPropertyName("back_transparent")]
    string? BackTransparent,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault);

public readonly record struct GenerationII(
    [property: JsonPropertyName("crystal")]
    Crystal Crystal,
    [property: JsonPropertyName("gold")] Gold Gold,
    [property: JsonPropertyName("silver")] Silver Silver
);

public readonly record struct Crystal(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_transparent")]
    string? BackShinyTransparent,
    [property: JsonPropertyName("back_transparent")]
    string? BackTransparent,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_transparent")]
    string? FrontShinyTransparent,
    [property: JsonPropertyName("front_transparent")]
    string? FrontTransparent
);

public readonly record struct Gold(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_transparent")]
    string? FrontTransparent
);

public readonly record struct Silver(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_transparent")]
    string? FrontTransparent
);

public readonly record struct GenerationIII(
    [property: JsonPropertyName("emerald")]
    Emerald Emerald,
    [property: JsonPropertyName("firered_leafgreen")]
    FireredLeafgreen FireredLeafgreen,
    [property: JsonPropertyName("ruby_sapphire")]
    RubySapphire RubySapphire
);

public readonly record struct Emerald(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny
);

public readonly record struct FireredLeafgreen(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny
);

public readonly record struct RubySapphire(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny
);

public readonly record struct GenerationIV(
    [property: JsonPropertyName("diamond_pearl")]
    DiamondPearl DiamondPearl,
    [property: JsonPropertyName("heartgold_soulsilver")]
    HeartgoldSoulsilver HeartgoldSoulsilver,
    [property: JsonPropertyName("platinum")]
    Platinum Platinum
);

public readonly record struct DiamondPearl(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_female")]
    string? BackFemale,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_female")]
    string? BackShinyFemale,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct HeartgoldSoulsilver(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_female")]
    string? BackFemale,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_female")]
    string? BackShinyFemale,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct Platinum(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_female")]
    string? BackFemale,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_female")]
    string? BackShinyFemale,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct GenerationV(
    [property: JsonPropertyName("black_white")]
    BlackWhite BlackWhite
);

public readonly record struct BlackWhite(
    [property: JsonPropertyName("animated")]
    Animated Animated,
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_female")]
    string? BackFemale,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_female")]
    string? BackShinyFemale,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct Animated(
    [property: JsonPropertyName("back_default")]
    string? BackDefault,
    [property: JsonPropertyName("back_female")]
    string? BackFemale,
    [property: JsonPropertyName("back_shiny")]
    string? BackShiny,
    [property: JsonPropertyName("back_shiny_female")]
    string? BackShinyFemale,
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct GenerationVI(
    [property: JsonPropertyName("omegaruby_alphasapphire")]
    OmegarubyAlphasapphire OmegarubyAlphasapphire,
    [property: JsonPropertyName("x_y")] XY XY
);

public readonly record struct OmegarubyAlphasapphire(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct XY(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct GenerationVII(
    [property: JsonPropertyName("icons")] Icons Icons,
    [property: JsonPropertyName("ultra_sun_ultra_moon")]
    UltraSunUltraMoon UltraSunUltraMoon
);

public readonly record struct Icons(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale
);

public readonly record struct UltraSunUltraMoon(
    [property: JsonPropertyName("front_default")]
    string? FrontDefault,
    [property: JsonPropertyName("front_female")]
    string? FrontFemale,
    [property: JsonPropertyName("front_shiny")]
    string? FrontShiny,
    [property: JsonPropertyName("front_shiny_female")]
    string? FrontShinyFemale
);

public readonly record struct GenerationVIII(
    [property: JsonPropertyName("icons")] Icons Icons
);