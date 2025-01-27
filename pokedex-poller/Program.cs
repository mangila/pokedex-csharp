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
var workers = new Dictionary<string, bool>();

var onWorkerStarted = (string generation, bool done) => { workers.Add(generation, done); };

var onWorkerCompleted = (string generation, bool done) =>
{
    workers[generation] = done;
    if (workers.Values.All(isDone => isDone))
    {
        cts.Cancel();
    }
};

foreach (var pokemonGeneration in PokemonGeneration.ToArray())
{
    builder.Services.AddSingleton<IHostedService>(provider => new Worker(
        logger: provider.GetRequiredService<ILogger<Worker>>(),
        workerOption: provider.GetRequiredService<IOptions<WorkerOption>>(),
        pokeApiOption: provider.GetRequiredService<IOptions<PokeApiOption>>(),
        pokemonGeneration: pokemonGeneration,
        pokemonHttpClient: provider.GetRequiredService<PokemonHttpClient>(),
        mongoDbCommandService: provider.GetRequiredService<MongoDbCommandService>(),
        mongoDbGridFsCommandService: provider.GetRequiredService<MongoDbGridFsCommandService>(),
        random: new Random(),
        onWorkerStarted: onWorkerStarted,
        onWorkerCompleted: onWorkerCompleted));
}

var host = builder.Build();
await host.RunAsync(cts.Token);