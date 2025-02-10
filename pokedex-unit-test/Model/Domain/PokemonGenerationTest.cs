using FluentAssertions;
using pokedex_shared.Common.Attributes;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonGeneration))]
public class PokemonGenerationTest
{
    [Test]
    [HappyPath]
    public void Test()
    {
        var generations = PokemonGeneration.ToArray();
        generations.Should().HaveCount(9);
        foreach (var generation in generations)
        {
            Action act = () => PokemonGeneration.From(generation.Value);
            act.Should().NotThrow<NotSupportedException>();
        }
    }

    [Test]
    [SadPath]
    public void TestInvalid()
    {
        Action act = () => PokemonGeneration.From("INVALID_GENERATION");
        act.Should().Throw<NotSupportedException>();
    }
}