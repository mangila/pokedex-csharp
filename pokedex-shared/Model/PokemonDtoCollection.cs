namespace pokedex_shared.Model;

public record PokemonDtoCollection(List<PokemonDto> pokemons)
{
    public PokemonDtoCollection() : this([])
    {
    }
}