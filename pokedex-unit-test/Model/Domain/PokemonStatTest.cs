using FluentAssertions;
using pokedex_shared.Common.Attributes;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonStat))]
public class PokemonStatTest
{
    [Test]
    [HappyPath]
    public void Test()
    {
        var stats = PokemonStat.ToArray();
        stats.Should().HaveCount(6);
        foreach (var stat in stats)
        {
            Action act = () => PokemonStat.From(stat.Name);
            act.Should().NotThrow<NotSupportedException>();
        }
    }

    [Test]
    [SadPath]
    public void TestNotValid()
    {
        Action act = () => PokemonStat.From("INVALID_STAT");
        act.Should().Throw<NotSupportedException>();
    }
}