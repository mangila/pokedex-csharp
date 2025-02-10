using FluentAssertions;
using pokedex_shared.Common.Attributes;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonType))]
public class PokemonTypeTest
{
    [Test]
    [HappyPath]
    public void Test()
    {
        var types = PokemonType.ToArray();
        types.Should().HaveCount(18);
        foreach (var type in types)
        {
            Action act = () => PokemonType.From(type.Value);
            act.Should().NotThrow<NotSupportedException>();
        }
    }

    [Test]
    [SadPath]
    public void TestNotValid()
    {
        Action act = () => PokemonType.From("INVALID_TYPE");
        act.Should().Throw<NotSupportedException>();
    }
}