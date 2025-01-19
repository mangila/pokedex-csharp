namespace pokedex_shared.Model.Dto;

public readonly record struct PokemonDtoCollection(List<PokemonDto> pokemons)
{
    public PokemonDtoCollection() : this([])
    {
    }
}