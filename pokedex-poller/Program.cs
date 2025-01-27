using Microsoft.Extensions.Options;
using pokedex_poller;
using pokedex_shared.Config;
using pokedex_shared.Http;
using pokedex_shared.Model.Domain;
using pokedex_shared.Option;
using pokedex_shared.Service;
using pokedex_shared.Service.Command;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
// Load Serilog from configuration
// Configure Serilog with the loaded configuration
builder.Services.AddSerilog(config =>
    config.ReadFrom.Configuration(builder.Configuration));
// Add Option Services to DI Container;
builder.Services.AddOptions<WorkerOption>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(WorkerOption)))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<PokedexApiOption>()
    .Bind(builder.Configuration.GetRequiredSection(nameof(PokedexApiOption)))
    .ValidateDataAnnotations()
    .ValidateOnStart();
// Add Services to the DI Container
// Configure Redis
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;
    redisOptions.InstanceName = "pokedex-poller:development:";
});
builder.Services.AddPokemonApi(builder.Configuration.GetRequiredSection(nameof(PokeApiOption)));
builder.Services.AddMongoDbCommandService(builder.Configuration.GetRequiredSection(nameof(MongoDbOption)));
builder.Services.AddSingleton<RedisService>();

var cts = new CancellationTokenSource();
var completedWorkers = new Dictionary<PokemonGeneration, bool>();

var onWorkerCompleted = (PokemonGeneration generation, bool done) =>
{
    completedWorkers[generation] = done;
    if (completedWorkers.Values.All(isDone => isDone))
    {
        cts.Cancel();
    }
};

foreach (var pokemonGeneration in PokemonGeneration.ToArray())
{
    builder.Services.AddSingleton<IHostedService>(provider =>
    {
        completedWorkers.Add(pokemonGeneration, false);
        var logger = provider.GetRequiredService<ILogger<Worker>>();
        var workerOption = provider.GetRequiredService<IOptions<WorkerOption>>();
        var pokeApiOption = provider.GetRequiredService<IOptions<PokeApiOption>>();
        var pokemonClient = provider.GetRequiredService<PokemonHttpClient>();
        var mongoDbService = provider.GetRequiredService<MongoDbCommandService>();
        var mongoDbGridFsService = provider.GetRequiredService<MongoDbGridFsCommandService>();
        return new Worker(logger,
            workerOption.Value,
            pokeApiOption.Value,
            pokemonGeneration,
            pokemonClient,
            mongoDbService,
            mongoDbGridFsService,
            new Random(),
            onWorkerCompleted);
    });
}

var host = builder.Build();
await host.RunAsync(cts.Token);