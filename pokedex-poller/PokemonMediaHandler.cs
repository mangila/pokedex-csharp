using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using pokedex_shared.Common.Option;
using pokedex_shared.Database.Command;
using pokedex_shared.Integration.PokeApi.Response.Pokemon;
using pokedex_shared.Model.Document.Embedded;
using pokedex_shared.Model.Domain;
using pokedex_shared.Service;

namespace pokedex_poller;

/**
 * <summary>
 *  Fetches all the media for the pokemon
 * </summary>
 */
public class PokemonMediaHandler(
    ILogger<PokemonMediaHandler> logger,
    IOptions<WorkerOption> workerOption,
    RedisService redisService,
    MongoDbGridFsCommandRepository mongoDbGridFsCommandRepository,
    IHttpClientFactory httpClientFactory)
{
    private const string CacheKeySpritesPrefix = "pokeapi.co:sprites:";
    private const string CacheKeyAudioPrefix = "pokeapi.co:audio:";

    private readonly DistributedCacheEntryOptions _options = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3)
    };

    private readonly WorkerOption _workerOption = workerOption.Value;
    private readonly Random _random = new();

    public async Task<List<PokemonMediaDocument>> FetchImagesAsync(
        PokemonName name,
        Sprites sprites,
        CancellationToken cancellationToken = default)
    {
        List<Task<PokemonMediaEntry>> entries =
        [
            FetchMediaAsync(name, sprites.FrontDefault!, "front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.BackDefault!, "back-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.FrontFemale!, "front-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.BackFemale!, "back-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.FrontShiny!, "front-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.BackShiny!, "back-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.FrontShinyFemale!, "front-shiny-female", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.BackShinyFemale!, "back-shiny-female", CacheKeySpritesPrefix,
                cancellationToken),
            // Home name, sprites
            FetchMediaAsync(name, sprites.Other.Home.FrontDefault!, "home-front-default", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Home.FrontFemale!, "home-front-female", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Home.FrontShiny!, "home-front-shiny", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Home.FrontShinyFemale!, "home-front-shiny-female",
                CacheKeySpritesPrefix, cancellationToken),
            // Official Artwork name, sprites
            FetchMediaAsync(name, sprites.Other.OfficialArtwork.FrontDefault!, "official-artwork-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Other.OfficialArtwork.FrontShiny!, "official-artwork-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            // DreamWorld name, sprites
            FetchMediaAsync(name, sprites.Other.DreamWorld.FrontDefault!, "dreamworld-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Other.DreamWorld.FrontFemale!, "dreamworld-front-female",
                CacheKeySpritesPrefix, cancellationToken),
            // Showdown name, sprites
            FetchMediaAsync(name, sprites.Other.Showdown.FrontDefault!, "showdown-front-default", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Showdown.BackDefault!, "showdown-back-default", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Showdown.FrontShiny!, "showdown-front-shiny", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Showdown.BackShiny!, "showdown-back-shiny", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Showdown.FrontFemale!, "showdown-front-female", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Showdown.BackFemale!, "showdown-back-female", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Other.Showdown.FrontShinyFemale!, "showdown-front-shiny-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Other.Showdown.BackShinyFemale!, "showdown-back-shiny-female",
                CacheKeySpritesPrefix, cancellationToken),
            // Generation I
            FetchMediaAsync(name, sprites.Versions.GenerationI.RedBlue.BackDefault!, "redblue-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.RedBlue.BackGray!, "redblue-back-gray",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.RedBlue.BackTransparent!, "redblue-back-transparent",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.RedBlue.FrontDefault!, "redblue-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.RedBlue.FrontGray!, "redblue-front-gray",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.RedBlue.FrontTransparent!, "redblue-front-transparent",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.Yellow.BackDefault!, "yellow-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.Yellow.BackGray!, "yellow-back-gray",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.Yellow.BackTransparent!, "yellow-back-transparent",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationI.Yellow.FrontDefault!, "yellow-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            // Generation II
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.BackDefault!, "crystal-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.BackShiny!, "crystal-back-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.BackShinyTransparent!,
                "crystal-back-shiny-transparent", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.BackTransparent!, "crystal-back-transparent",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.FrontDefault!, "crystal-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.FrontShiny!, "crystal-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.FrontShinyTransparent!,
                "crystal-front-shiny-transparent", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Crystal.FrontTransparent!, "crystal-front-transparent",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Gold.BackDefault!, "gold-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Gold.BackShiny!, "gold-back-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Gold.FrontDefault!, "gold-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Gold.FrontShiny!, "gold-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Gold.FrontTransparent!, "gold-front-transparent",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Silver.BackDefault!, "silver-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Silver.BackShiny!, "silver-back-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Silver.FrontDefault!, "silver-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Silver.FrontShiny!, "silver-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationII.Silver.FrontTransparent!, "silver-front-transparent",
                CacheKeySpritesPrefix, cancellationToken),
            // Generation III
            FetchMediaAsync(name, sprites.Versions.GenerationIII.Emerald.FrontDefault!, "emerald-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.Emerald.FrontShiny!, "emerald-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.FireredLeafgreen.BackDefault!,
                "firered-leafgreen-back-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.FireredLeafgreen.BackShiny!,
                "firered-leafgreen-back-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.FireredLeafgreen.FrontDefault!,
                "firered-leafgreen-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.FireredLeafgreen.FrontShiny!,
                "firered-leafgreen-front-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.RubySapphire.BackDefault!,
                "ruby-sapphire-back-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.RubySapphire.BackShiny!, "ruby-sapphire-back-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.RubySapphire.FrontDefault!,
                "ruby-sapphire-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIII.RubySapphire.FrontShiny!, "ruby-sapphire-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            // Generation IV
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.BackDefault!, "diamond-pearl-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.BackFemale!, "diamond-pearl-back-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.BackShiny!, "diamond-pearl-back-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.BackShinyFemale!,
                "diamond-pearl-back-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.FrontDefault!,
                "diamond-pearl-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.FrontFemale!, "diamond-pearl-front-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.FrontShiny!, "diamond-pearl-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.DiamondPearl.FrontShinyFemale!,
                "diamond-pearl-front-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackDefault!,
                "heartgold-soulsilver-back-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackFemale!,
                "heartgold-soulsilver-back-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackShiny!,
                "heartgold-soulsilver-back-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.BackShinyFemale!,
                "heartgold-soulsilver-back-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontDefault!,
                "heartgold-soulsilver-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontFemale!,
                "heartgold-soulsilver-front-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontShiny!,
                "heartgold-soulsilver-front-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.HeartgoldSoulsilver.FrontShinyFemale!,
                "heartgold-soulsilver-front-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.BackDefault!, "platinum-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.BackFemale!, "platinum-back-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.BackShiny!, "platinum-back-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.BackShinyFemale!, "platinum-back-shiny-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.FrontDefault!, "platinum-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.FrontFemale!, "platinum-front-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.FrontShiny!, "platinum-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationIV.Platinum.FrontShinyFemale!,
                "platinum-front-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            // Generation V
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackDefault!,
                "blackwhite-animated-back-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackFemale!,
                "blackwhite-animated-back-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackShiny!,
                "blackwhite-animated-back-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.BackShinyFemale!,
                "blackwhite-animated-back-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontDefault!,
                "blackwhite-animated-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontFemale!,
                "blackwhite-animated-front-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontShiny!,
                "blackwhite-animated-front-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.Animated.FrontShinyFemale!,
                "blackwhite-animated-front-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.BackDefault!, "blackwhite-back-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.BackFemale!, "blackwhite-back-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.BackShiny!, "blackwhite-back-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.BackShinyFemale!,
                "blackwhite-back-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.FrontDefault!, "blackwhite-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.FrontFemale!, "blackwhite-front-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.FrontShiny!, "blackwhite-front-shiny",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationV.BlackWhite.FrontShinyFemale!,
                "blackwhite-front-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            // Generation VI
            FetchMediaAsync(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontDefault!,
                "omegaruby-alphasapphire-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontFemale!,
                "omegaruby-alphasapphire-front-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontShiny!,
                "omegaruby-alphasapphire-front-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVI.OmegarubyAlphasapphire.FrontShinyFemale!,
                "omegaruby-alphasapphire-front-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVI.XY.FrontDefault!, "xy-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVI.XY.FrontFemale!, "xy-front-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVI.XY.FrontShiny!, "xy-front-shiny", CacheKeySpritesPrefix,
                cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVI.XY.FrontShinyFemale!, "xy-front-shiny-female",
                CacheKeySpritesPrefix, cancellationToken),
            // Generation VII
            FetchMediaAsync(name, sprites.Versions.GenerationVII.Icons.FrontDefault!, "icons-front-default",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVII.Icons.FrontFemale!, "icons-front-female",
                CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontDefault!,
                "ultrasun-ultramoon-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontFemale!,
                "ultrasun-ultramoon-front-female", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontShiny!,
                "ultrasun-ultramoon-front-shiny", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVII.UltraSunUltraMoon.FrontShinyFemale!,
                "ultrasun-ultramoon-front-shiny-female", CacheKeySpritesPrefix, cancellationToken),
            // Generation VIII
            FetchMediaAsync(name, sprites.Versions.GenerationVIII.Icons.FrontDefault!,
                "generationviii-icons-front-default", CacheKeySpritesPrefix, cancellationToken),
            FetchMediaAsync(name, sprites.Versions.GenerationVIII.Icons.FrontFemale!,
                "generationviii-icons-front-female", CacheKeySpritesPrefix, cancellationToken),
        ];
        var fetchedEntries = await Task.WhenAll(entries);
        return await InsertEntriesAsync(fetchedEntries, cancellationToken);
    }


    public async Task<List<PokemonMediaDocument>> FetchAudiosAsync(
        PokemonName name,
        Cries cries,
        CancellationToken cancellationToken = default)
    {
        List<Task<PokemonMediaEntry>> entries =
        [
            FetchMediaAsync(name, cries.Legacy!, "legacy", CacheKeyAudioPrefix, cancellationToken),
            FetchMediaAsync(name, cries.Latest!, "latest", CacheKeyAudioPrefix, cancellationToken),
        ];
        var fetchedEntries = await Task.WhenAll(entries);
        return await InsertEntriesAsync(fetchedEntries, cancellationToken);
    }

    private async Task<List<PokemonMediaDocument>> InsertEntriesAsync(
        PokemonMediaEntry[] entries,
        CancellationToken cancellationToken = default)
    {
        var documents = new List<PokemonMediaDocument>();
        foreach (var entry in entries)
        {
            if (entry != default)
            {
                var document = await mongoDbGridFsCommandRepository.InsertAsync(
                    entry: entry,
                    cancellationToken: cancellationToken);
                documents.Add(document);
            }
        }

        return documents;
    }

    private async Task<PokemonMediaEntry> FetchMediaAsync(
        PokemonName name,
        string uri,
        string description,
        string cacheKeyPrefix,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrEmpty(uri))
        {
            return default;
        }

        var mediaUri = new Uri(uri);
        var cacheKey = string.Concat(cacheKeyPrefix, mediaUri.AbsolutePath);
        var cacheValue = await redisService.GetAsync(
            cacheKey,
            cancellationToken);

        if (cacheValue is null)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(GetJitter()), cancellationToken);
            var httpClient = httpClientFactory.CreateClient();
            using var response = await httpClient.GetAsync(mediaUri, cancellationToken);
            response.EnsureSuccessStatusCode();
            var file = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            await redisService.SetAsync(
                key: cacheKey,
                data: file,
                options: _options,
                cancellationToken: cancellationToken);
            return new PokemonMediaEntry(
                name,
                mediaUri,
                description,
                file
            );
        }

        return new PokemonMediaEntry(
            name,
            new Uri(uri),
            description,
            cacheValue
        );
    }

    private int GetJitter()
    {
        return _random.Next(
            _workerOption.MediaHandler.Interval.Min,
            _workerOption.MediaHandler.Interval.Max);
    }
}