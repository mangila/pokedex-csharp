using Microsoft.Extensions.Options;
using pokedex_shared.Http.Pokemon;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;
using pokedex_shared.Service.Command;

namespace pokedex_poller;

/**
 * <summary>
 *  Fetches all the media for the pokemon
 * </summary>
 */
public class MediaHandler(
    ILogger<MediaHandler> logger,
    IOptions<WorkerOption> workerOption,
    MongoDbGridFsCommandService mongoDbGridFsCommandService)
{
    private readonly WorkerOption _workerOption = workerOption.Value;
    private readonly Random _random = new();

    public async Task<List<PokemonMediaDocument>> FetchImagesAsync(
        PokemonName name,
        Sprites sprites,
        CancellationToken cancellationToken = default)
    {
        List<PokemonMediaEntry> entries =
        [
            GetMedia(name, sprites.FrontDefault!, "FrontDefault"),
            GetMedia(name, sprites.BackDefault!, "BackDefault"),
            GetMedia(name, sprites.FrontFemale!, "FrontFemale"),
            GetMedia(name, sprites.BackFemale!, "BackFemale"),
            GetMedia(name, sprites.FrontShiny!, "FrontShiny"),
            GetMedia(name, sprites.BackShiny!, "BackShiny"),
            GetMedia(name, sprites.FrontShinyFemale!, "FrontShinyFemale"),
            GetMedia(name, sprites.BackShinyFemale!, "BackShinyFemale"),

            // Home name,sprites
            GetMedia(name, sprites.Other.Home.FrontDefault!, "HomeFrontDefault"),
            GetMedia(name, sprites.Other.Home.FrontFemale!, "HomeFrontFemale"),
            GetMedia(name, sprites.Other.Home.FrontShiny!, "HomeFrontShiny"),
            GetMedia(name, sprites.Other.Home.FrontShinyFemale!, "HomeFrontShinyFemale"),

            // Official Artwork name,sprites
            GetMedia(name, sprites.Other.OfficialArtwork.FrontDefault!, "OfficialArtworkFrontDefault"),

            GetMedia(name, sprites.Other.OfficialArtwork.FrontShiny!, "OfficialArtworkFrontShiny"),

            // DreamWorld name,sprites
            GetMedia(name, sprites.Other.DreamWorld.FrontDefault!, "DreamWorldFrontDefault"),
            GetMedia(name, sprites.Other.DreamWorld.FrontFemale!, "DreamWorldFrontFemale"),

            // Showdown name,sprites
            GetMedia(name, sprites.Other.Showdown.FrontDefault!, "ShowdownFrontDefault"),
            GetMedia(name, sprites.Other.Showdown.BackDefault!, "ShowdownBackDefault"),
            GetMedia(name, sprites.Other.Showdown.FrontShiny!, "ShowdownFrontShiny"),
            GetMedia(name, sprites.Other.Showdown.BackShiny!, "ShowdownBackShiny"),
            GetMedia(name, sprites.Other.Showdown.FrontFemale!, "ShowdownFrontFemale"),
            GetMedia(name, sprites.Other.Showdown.BackFemale!, "ShowdownBackFemale"),
            GetMedia(name, sprites.Other.Showdown.FrontShinyFemale!, "ShowdownFrontShinyFemale"),
            GetMedia(name, sprites.Other.Showdown.BackShinyFemale!, "ShowdownBackShinyFemale"),

            // Generation I
            GetMedia(name, sprites.Versions.GenerationI.RedBlue.BackDefault!, "RedBlueBackDefault"),
            GetMedia(name, sprites.Versions.GenerationI.RedBlue.BackGray!, "RedBlueBackGray"),
            GetMedia(name, sprites.Versions.GenerationI.RedBlue.BackTransparent!, "RedBlueBackTransparent"),

            GetMedia(name, sprites.Versions.GenerationI.RedBlue.FrontDefault!, "RedBlueFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationI.RedBlue.FrontGray!, "RedBlueFrontGray"),
            GetMedia(name, sprites.Versions.GenerationI.RedBlue.FrontTransparent!, "RedBlueFrontTransparent"),

            GetMedia(name, sprites.Versions.GenerationI.Yellow.BackDefault!, "YellowBackDefault"),
            GetMedia(name, sprites.Versions.GenerationI.Yellow.BackGray!, "YellowBackGray"),
            GetMedia(name, sprites.Versions.GenerationI.Yellow.BackTransparent!, "YellowBackTransparent"),

            GetMedia(name, sprites.Versions.GenerationI.Yellow.FrontDefault!, "YellowFrontDefault"),

            // Generation II
            GetMedia(name, sprites.Versions.GenerationII.Crystal.BackDefault!, "CrystalBackDefault"),
            GetMedia(name, sprites.Versions.GenerationII.Crystal.BackShiny!, "CrystalBackShiny"),
            GetMedia(name, sprites.Versions.GenerationII.Crystal.BackShinyTransparent!, "CrystalBackShinyTransparent"),

            GetMedia(name, sprites.Versions.GenerationII.Crystal.BackTransparent!, "CrystalBackTransparent"),

            GetMedia(name, sprites.Versions.GenerationII.Crystal.FrontDefault!, "CrystalFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationII.Crystal.FrontShiny!, "CrystalFrontShiny"),
            GetMedia(name, sprites.Versions.GenerationII.Crystal.FrontShinyTransparent!,
                "CrystalFrontShinyTransparent"),

            GetMedia(name, sprites.Versions.GenerationII.Crystal.FrontTransparent!, "CrystalFrontTransparent"),

            GetMedia(name, sprites.Versions.GenerationII.Gold.BackDefault!, "GoldBackDefault"),
            GetMedia(name, sprites.Versions.GenerationII.Gold.BackShiny!, "GoldBackShiny"),
            GetMedia(name, sprites.Versions.GenerationII.Gold.FrontDefault!, "GoldFrontDefault"),
            GetMedia(name, sprites.Versions.GenerationII.Gold.FrontShiny!, "GoldFrontShiny"),
            GetMedia(name, sprites.Versions.GenerationII.Gold.FrontTransparent!, "GoldFrontTransparent"),

            GetMedia(name, sprites.Versions.GenerationII.Silver.BackDefault!, "SilverBackDefault"),
            GetMedia(name, sprites.Versions.GenerationII.Silver.BackShiny!, "SilverBackShiny"),
            GetMedia(name, sprites.Versions.GenerationII.Silver.FrontDefault!, "SilverFrontDefault"),
            GetMedia(name, sprites.Versions.GenerationII.Silver.FrontShiny!, "SilverFrontShiny"),
            GetMedia(name, sprites.Versions.GenerationII.Silver.FrontTransparent!, "SilverFrontTransparent"),

            // Generation III

            GetMedia(name, sprites.Versions.GenerationIII.Emerald.FrontDefault!, "EmeraldFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationIII.Emerald.FrontShiny!, "EmeraldFrontShiny"),
            GetMedia(name, sprites.Versions.GenerationIII.FireredLeafgreen.BackDefault!, "FireredLeafgreenBackDefault"),

            GetMedia(name, sprites.Versions.GenerationIII.FireredLeafgreen.BackShiny!, "FireredLeafgreenBackShiny"),

            GetMedia(name, sprites.Versions.GenerationIII.FireredLeafgreen.FrontDefault!,
                "FireredLeafgreenFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationIII.FireredLeafgreen.FrontShiny!, "FireredLeafgreenFrontShiny"),

            GetMedia(name, sprites.Versions.GenerationIII.RubySapphire.BackDefault!, "RubySapphireBackDefault"),

            GetMedia(name, sprites.Versions.GenerationIII.RubySapphire.BackShiny!, "RubySapphireBackShiny"),

            GetMedia(name, sprites.Versions.GenerationIII.RubySapphire.FrontDefault!, "RubySapphireFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationIII.RubySapphire.FrontShiny!, "RubySapphireFrontShiny"),

            // Generation IV

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.BackDefault!, "DiamondPearlBackDefault"),

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.BackFemale!, "DiamondPearlBackFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.BackShiny!, "DiamondPearlBackShiny"),

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.BackShinyFemale!, "DiamondPearlBackShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.FrontDefault!, "DiamondPearlFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.FrontFemale!, "DiamondPearlFrontFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.FrontShiny!, "DiamondPearlFrontShiny"),

            GetMedia(name, sprites.Versions.GenerationIV.DiamondPearl.FrontShinyFemale!,
                "DiamondPearlFrontShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackDefault!,
                "HeartgoldSoulsilverBackDefault"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackFemale!,
                "HeartgoldSoulsilverBackFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackShiny!,
                "HeartgoldSoulsilverBackShiny"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackShinyFemale!,
                "HeartgoldSoulsilverBackShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontDefault!,
                "HeartgoldSoulsilverFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontFemale!,
                "HeartgoldSoulsilverFrontFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontShiny!,
                "HeartgoldSoulsilverFrontShiny"),

            GetMedia(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontShinyFemale!,
                "HeartgoldSoulsilverFrontShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.Platinum.BackDefault!, "PlatinumBackDefault"),

            GetMedia(name, sprites.Versions.GenerationIV.Platinum.BackFemale!, "PlatinumBackFemale"),
            GetMedia(name, sprites.Versions.GenerationIV.Platinum.BackShiny!, "PlatinumBackShiny"),
            GetMedia(name, sprites.Versions.GenerationIV.Platinum.BackShinyFemale!, "PlatinumBackShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.Platinum.FrontDefault!, "PlatinumFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationIV.Platinum.FrontFemale!, "PlatinumFrontFemale"),

            GetMedia(name, sprites.Versions.GenerationIV.Platinum.FrontShiny!, "PlatinumFrontShiny"),
            GetMedia(name, sprites.Versions.GenerationIV.Platinum.FrontShinyFemale!, "PlatinumFrontShinyFemale"),

            // Generation V

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackDefault!,
                "BlackWhiteAnimatedBackDefault"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackFemale!,
                "BlackWhiteAnimatedBackFemale"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackShiny!, "BlackWhiteAnimatedBackShiny"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackShinyFemale!,
                "BlackWhiteAnimatedBackShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontDefault!,
                "BlackWhiteAnimatedFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontFemale!,
                "BlackWhiteAnimatedFrontFemale"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontShiny!,
                "BlackWhiteAnimatedFrontShiny"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontShinyFemale!,
                "BlackWhiteAnimatedFrontShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.BackDefault!, "BlackWhiteBackDefault"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.BackFemale!, "BlackWhiteBackFemale"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.BackShiny!, "BlackWhiteBackShiny"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.BackShinyFemale!, "BlackWhiteBackShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.FrontDefault!, "BlackWhiteFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.FrontFemale!, "BlackWhiteFrontFemale"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.FrontShiny!, "BlackWhiteFrontShiny"),

            GetMedia(name, sprites.Versions.GenerationV.BlackWhite.FrontShinyFemale!, "BlackWhiteFrontShinyFemale"),

            // Generation VI

            GetMedia(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontDefault!,
                "OmegarubyAlphasapphireFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontFemale!,
                "OmegarubyAlphasapphireFrontFemale"),

            GetMedia(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontShiny!,
                "OmegarubyAlphasapphireFrontShiny"),

            GetMedia(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontShinyFemale!,
                "OmegarubyAlphasapphireFrontShinyFemale"),

            GetMedia(name, sprites.Versions.GenerationVI.XY.FrontDefault!, "XYFrontDefault"),
            GetMedia(name, sprites.Versions.GenerationVI.XY.FrontFemale!, "XYFrontFemale"),
            GetMedia(name, sprites.Versions.GenerationVI.XY.FrontShiny!, "XYFrontShiny"),
            GetMedia(name, sprites.Versions.GenerationVI.XY.FrontShinyFemale!, "XYFrontShinyFemale"),

            // Generation VII
            GetMedia(name, sprites.Versions.GenerationVII.Icons.FrontDefault!, "IconsFrontDefault"),
            GetMedia(name, sprites.Versions.GenerationVII.Icons.FrontFemale!, "IconsFrontFemale"),
            GetMedia(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontDefault!,
                "UltraSunUltraMoonFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontFemale!,
                "UltraSunUltraMoonFrontFemale"),

            GetMedia(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontShiny!, "UltraSunUltraMoonFrontShiny"),

            GetMedia(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontShinyFemale!,
                "UltraSunUltraMoonFrontShinyFemale"),

            // Generation VIII

            GetMedia(name, sprites.Versions.GenerationVIII.Icons.FrontDefault!, "GenerationVIIIIconsFrontDefault"),

            GetMedia(name, sprites.Versions.GenerationVIII.Icons.FrontFemale!, "GenerationVIIIIconsFrontFemale")
        ];


        var mediaDocuments = new List<PokemonMediaDocument>();
        foreach (var entry in entries)
        {
            if (entry != default)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(GetJitter()), cancellationToken);
                mediaDocuments.Add(
                    await mongoDbGridFsCommandService.InsertAsync(
                        entry,
                        cancellationToken)
                );
            }
        }

        return mediaDocuments;
    }


    public async Task<List<PokemonMediaDocument>> FetchAudiosAsync(
        PokemonName name,
        Cries cries,
        CancellationToken cancellationToken = default)
    {
        List<PokemonMediaEntry> entries =
        [
            GetMedia(name, cries.Legacy!, "Legacy"),
            GetMedia(name, cries.Latest!, "Latest"),
        ];

        var mediaDocuments = new List<PokemonMediaDocument>();
        foreach (var entry in entries)
        {
            if (entry != default)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(GetJitter()), cancellationToken);
                mediaDocuments.Add(
                    await mongoDbGridFsCommandService.InsertAsync(
                        entry,
                        cancellationToken)
                );
            }
        }

        return mediaDocuments;
    }

    private static PokemonMediaEntry GetMedia(
        PokemonName pokemonName,
        string uri,
        string description)
    {
        if (string.IsNullOrEmpty(uri))
        {
            return default;
        }

        return new PokemonMediaEntry(
            Name: pokemonName,
            Uri: new Uri(uri),
            Description: description);
    }

    private int GetJitter()
    {
        return _random.Next(
            _workerOption.MediaHandler.Interval.Min,
            _workerOption.MediaHandler.Interval.Max);
    }
}