namespace pokedex_shared.Model;

public readonly record struct PokemonDtoCollection(List<PokemonDto> pokemons)
{
    public PokemonDtoCollection() : this([])
    {
    }
}