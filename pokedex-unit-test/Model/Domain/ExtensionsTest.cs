using FluentAssertions;
using pokedex_shared.Model.Domain;

namespace pokedex_unit_test.Model.Domain;

[TestFixture]
[TestOf(typeof(Extensions))]
public class ExtensionsTest
{
    [Test]
    public void TestPokemonMediaEntry()
    {
        var entry = new PokemonMediaEntry(
            new PokemonName("abra"),
            new Uri("https://pokedex.io/abc.png"),
            "front-default",
            []
        );
        entry
            .Should()
            .NotBeNull();
        entry.GetFileName()
            .Should()
            .Be("abra-front-default.png");
        entry.GetFileExtension()
            .Should()
            .Be("png");
        entry.GetContentType()
            .Should()
            .Be("image/png");
    }

    [Test]
    public void Test()
    {
        Action act = () =>
        {
            var entry = new PokemonMediaEntry(
                new PokemonName("hejsan"),
                new Uri("https://pokedex.io/abc.xyz"),
                "description",
                []
            );
            entry.GetContentType();
        };
        act.Should().Throw<NotSupportedException>();
    }
}