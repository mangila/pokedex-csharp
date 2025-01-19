namespace pokedex_shared.Model;

public class PokemonIdCollection
{
    public readonly List<PokemonId> Ids;

    public PokemonIdCollection(List<int> ids)
    {
        Ids = ids.Select(id => new PokemonId(id.ToString())).ToList();
    }
}