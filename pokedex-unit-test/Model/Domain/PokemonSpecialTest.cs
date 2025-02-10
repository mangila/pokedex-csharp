using FluentAssertions;
using pokedex_shared.Common.Attributes;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonSpecial))]
public class PokemonSpecialTest
{
    [Test]
    [HappyPath]
    public void Test()
    {
        var specials = PokemonSpecial.ToArray();
        specials.Should().HaveCount(3);
        foreach (var special in specials)
        {
            Action act = () => PokemonSpecial.From(special.Value);
            act.Should().NotThrow<NotSupportedException>();
        }
    }

    [Test]
    [SadPath]
    public void TestNotValid()
    {
        Action act = () => PokemonSpecial.From("INVALID_SPECIAL");
        act.Should().Throw<NotSupportedException>();
    }
}