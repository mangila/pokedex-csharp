namespace pokedex_shared.Http.Pokemon;

public class PokemonApiResponse
{
    public Abilities[] abilities { get; set; }
    public int base_experience { get; set; }
    public Cries cries { get; set; }
    public Forms[] forms { get; set; }
    public Game_indices[] game_indices { get; set; }
    public int height { get; set; }
    public object[] held_items { get; set; }
    public int id { get; set; }
    public bool is_default { get; set; }
    public string location_area_encounters { get; set; }
    public Moves[] moves { get; set; }
    public string name { get; set; }
    public int order { get; set; }
    public object[] past_abilities { get; set; }
    public object[] past_types { get; set; }
    public Species species { get; set; }
    public Sprites sprites { get; set; }
    public Stats[] stats { get; set; }
    public Types[] types { get; set; }
    public int weight { get; set; }
}

public class Abilities
{
    public Ability ability { get; set; }
    public bool is_hidden { get; set; }
    public int slot { get; set; }
}

public class Ability
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Cries
{
    public string latest { get; set; }
    public string legacy { get; set; }
}

public class Forms
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Game_indices
{
    public int game_index { get; set; }
    public Version version { get; set; }
}

public class Version
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Moves
{
    public Move move { get; set; }
    public Version_group_details[] version_group_details { get; set; }
}

public class Move
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Version_group_details
{
    public int level_learned_at { get; set; }
    public Move_learn_method move_learn_method { get; set; }
    public Version_group version_group { get; set; }
}

public class Move_learn_method
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Version_group
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Species
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Sprites
{
    public string back_default { get; set; }
    public object back_female { get; set; }
    public string back_shiny { get; set; }
    public object back_shiny_female { get; set; }
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
    public Other other { get; set; }
    public Versions versions { get; set; }
}

public class Other
{
    public Dream_world dream_world { get; set; }
    public Home home { get; set; }
    public Official_artwork official_artwork { get; set; }
    public Showdown showdown { get; set; }
}

public class Dream_world
{
    public string front_default { get; set; }
    public object front_female { get; set; }
}

public class Home
{
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Official_artwork
{
    public string front_default { get; set; }
    public string front_shiny { get; set; }
}

public class Showdown
{
    public string back_default { get; set; }
    public object back_female { get; set; }
    public string back_shiny { get; set; }
    public object back_shiny_female { get; set; }
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Versions
{
    public Generation_i generation_i { get; set; }
    public Generation_ii generation_ii { get; set; }
    public Generation_iii generation_iii { get; set; }
    public Generation_iv generation_iv { get; set; }
    public Generation_v generation_v { get; set; }
    public Generation_vi generation_vi { get; set; }
    public Generation_vii generation_vii { get; set; }
    public Generation_viii generation_viii { get; set; }
}

public class Generation_i
{
    public Red_blue red_blue { get; set; }
    public Yellow yellow { get; set; }
}

public class Red_blue
{
    public string back_default { get; set; }
    public string back_gray { get; set; }
    public string back_transparent { get; set; }
    public string front_default { get; set; }
    public string front_gray { get; set; }
    public string front_transparent { get; set; }
}

public class Yellow
{
    public string back_default { get; set; }
    public string back_gray { get; set; }
    public string back_transparent { get; set; }
    public string front_default { get; set; }
    public string front_gray { get; set; }
    public string front_transparent { get; set; }
}

public class Generation_ii
{
    public Crystal crystal { get; set; }
    public Gold gold { get; set; }
    public Silver silver { get; set; }
}

public class Crystal
{
    public string back_default { get; set; }
    public string back_shiny { get; set; }
    public string back_shiny_transparent { get; set; }
    public string back_transparent { get; set; }
    public string front_default { get; set; }
    public string front_shiny { get; set; }
    public string front_shiny_transparent { get; set; }
    public string front_transparent { get; set; }
}

public class Gold
{
    public string back_default { get; set; }
    public string back_shiny { get; set; }
    public string front_default { get; set; }
    public string front_shiny { get; set; }
    public string front_transparent { get; set; }
}

public class Silver
{
    public string back_default { get; set; }
    public string back_shiny { get; set; }
    public string front_default { get; set; }
    public string front_shiny { get; set; }
    public string front_transparent { get; set; }
}

public class Generation_iii
{
    public Emerald emerald { get; set; }
    public Firered_leafgreen firered_leafgreen { get; set; }
    public Ruby_sapphire ruby_sapphire { get; set; }
}

public class Emerald
{
    public string front_default { get; set; }
    public string front_shiny { get; set; }
}

public class Firered_leafgreen
{
    public string back_default { get; set; }
    public string back_shiny { get; set; }
    public string front_default { get; set; }
    public string front_shiny { get; set; }
}

public class Ruby_sapphire
{
    public string back_default { get; set; }
    public string back_shiny { get; set; }
    public string front_default { get; set; }
    public string front_shiny { get; set; }
}

public class Generation_iv
{
    public Diamond_pearl diamond_pearl { get; set; }
    public Heartgold_soulsilver heartgold_soulsilver { get; set; }
    public Platinum platinum { get; set; }
}

public class Diamond_pearl
{
    public string back_default { get; set; }
    public object back_female { get; set; }
    public string back_shiny { get; set; }
    public object back_shiny_female { get; set; }
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Heartgold_soulsilver
{
    public string back_default { get; set; }
    public object back_female { get; set; }
    public string back_shiny { get; set; }
    public object back_shiny_female { get; set; }
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Platinum
{
    public string back_default { get; set; }
    public object back_female { get; set; }
    public string back_shiny { get; set; }
    public object back_shiny_female { get; set; }
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Generation_v
{
    public Black_white black_white { get; set; }
}

public class Black_white
{
    public Animated animated { get; set; }
    public string back_default { get; set; }
    public object back_female { get; set; }
    public string back_shiny { get; set; }
    public object back_shiny_female { get; set; }
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Animated
{
    public string back_default { get; set; }
    public object back_female { get; set; }
    public string back_shiny { get; set; }
    public object back_shiny_female { get; set; }
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Generation_vi
{
    public Omegaruby_alphasapphire omegaruby_alphasapphire { get; set; }
    public X_y x_y { get; set; }
}

public class Omegaruby_alphasapphire
{
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class X_y
{
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Generation_vii
{
    public Icons icons { get; set; }
    public Ultra_sun_ultra_moon ultra_sun_ultra_moon { get; set; }
}

public class Icons
{
    public string front_default { get; set; }
    public object front_female { get; set; }
}

public class Ultra_sun_ultra_moon
{
    public string front_default { get; set; }
    public object front_female { get; set; }
    public string front_shiny { get; set; }
    public object front_shiny_female { get; set; }
}

public class Generation_viii
{
    public Icons1 icons { get; set; }
}

public class Icons1
{
    public string front_default { get; set; }
    public object front_female { get; set; }
}

public class Stats
{
    public int base_stat { get; set; }
    public int effort { get; set; }
    public Stat stat { get; set; }
}

public class Stat
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Types
{
    public int slot { get; set; }
    public Type type { get; set; }
}

public class Type
{
    public string name { get; set; }
    public string url { get; set; }
}