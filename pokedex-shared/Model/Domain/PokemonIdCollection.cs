using System.Collections.Immutable;

namespace pokedex_shared.Model.Domain;

public readonly record struct PokemonIdCollection
{
    public readonly ImmutableList<PokemonId> Ids;

    public PokemonIdCollection(List<int> ids)
    {
        Ids = ids
            .Select(id => new PokemonId(id))
            .ToImmutableList();
    }
}