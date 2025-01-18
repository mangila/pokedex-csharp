using pokedex_shared.Model;

namespace pokedex_shared.Service;

public class PokemonService(DatasourceService datasource)
{
    public IEnumerable<PokemonDto> FindAllByIdAsync(List<int> ids,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PokemonDto> FindAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PokemonDto?> FindOneByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await datasource.FindAsync(id.ToString(), cancellationToken);
    }
}