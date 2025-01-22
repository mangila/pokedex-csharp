using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(PokemonName))]
public class PokemonNameTest
{
    [Test]
    public void Test()
    {
        // No special stuffs
        Action act = () => new PokemonName("#");
        act.Should().Throw<ValidationException>();
        // Except Mr.Mime
        act = () => new PokemonName("Mr.Mime");
        act.Should().NotThrow<ValidationException>();
        // No long strings
        string longString = string.Concat(Enumerable.Repeat("string", 30));
        act = () => new PokemonId(longString);
        act.Should().Throw<ValidationException>();
    }
}