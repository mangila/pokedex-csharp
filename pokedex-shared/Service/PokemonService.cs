using pokedex_shared.Model;

namespace pokedex_shared.Service;

public class PokemonService(DatasourceService datasource)
{
    public async Task<PokemonDtoCollection> FindAllByPokemonIdAsync(PokemonIdCollection pokemonIdCollection,
        CancellationToken cancellationToken = default)
    {
        return await datasource.FindAllByPokemonIdAsync(pokemonIdCollection,cancellationToken);
    }

    public async Task<PokemonDtoCollection> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await datasource.FindAllAsync(cancellationToken);
    }

    public async Task<PokemonDtoCollection> SearchByName(string search,
        CancellationToken cancellationToken = default)
    {
        return await datasource.SearchByName(search, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByPokemonIdAsync(PokemonId pokemonId,
        CancellationToken cancellationToken = default)
    {
        return await datasource.FindByPokemonIdAsync(pokemonId, cancellationToken);
    }

    public async Task<PokemonDto?> FindOneByNameAsync(PokemonName pokemonName,
        CancellationToken cancellationToken = default)
    {
        return await datasource.FindByNameAsync(pokemonName, cancellationToken);
    }
}