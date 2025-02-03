using Microsoft.Extensions.Options;
using pokedex_poller;
using pokedex_shared.Common.Option;
using pokedex_shared.Database;
using pokedex_shared.Database.Command;
using pokedex_shared.Integration.PokeApi;
using pokedex_shared.Model.Domain;
using pokedex_shared.Service;
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
builder.Services.AddPokeApi(builder.Configuration.GetRequiredSection(nameof(PokeApiOption)));
builder.Services.AddMongoDbCommandRepository(builder.Configuration.GetRequiredSection(nameof(MongoDbOption)));
builder.Services.AddSingleton<RedisService>();
builder.Services.AddSingleton<PokemonHandler>();
builder.Services.AddSingleton<PokemonMediaHandler>();

var cts = new CancellationTokenSource();
var workers = new Dictionary<string, bool>();

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
    workers.Add(pokemonGeneration.Value, false);
    builder.Services.AddSingleton<IHostedService>(provider => new Worker(
        logger: provider.GetRequiredService<ILogger<Worker>>(),
        workerOption: provider.GetRequiredService<IOptions<WorkerOption>>(),
        pokemonGeneration: pokemonGeneration,
        mongoDbCommandRepository: provider.GetRequiredService<MongoDbCommandRepository>(),
        pokemonHandler: provider.GetRequiredService<PokemonHandler>(),
        pokemonMediaHandler: provider.GetRequiredService<PokemonMediaHandler>(),
        onWorkerCompleted: onWorkerCompleted));
}

var host = builder.Build();
await host.RunAsync(cts.Token);