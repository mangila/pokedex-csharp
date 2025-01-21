using Microsoft.Extensions.Options;
using pokedex_poller;
using pokedex_shared.Config;
using pokedex_shared.Http;
using pokedex_shared.Option;
using pokedex_shared.Service;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);
// Load Serilog from configuration
// Configure Serilog with the loaded configuration
builder.Services.AddSerilog(config =>
    config.ReadFrom.Configuration(builder.Configuration));
// Add Option Services to DI Container
var sectionWorkerOption = builder
    .Configuration
    .GetRequiredSection(nameof(WorkerOption));
builder.Services.AddOptions<WorkerOption>()
    .Bind(sectionWorkerOption)
    .ValidateDataAnnotations()
    .ValidateOnStart();
var sectionPokemonOption = builder
    .Configuration
    .GetRequiredSection(nameof(PokeApiOption));
builder.Services.AddOptions<PokeApiOption>()
    .Bind(sectionPokemonOption)
    .ValidateDataAnnotations()
    .ValidateOnStart();
// Add Services to the DI Container
builder.Services.AddPokemonApi(builder.Configuration.GetSection(nameof(PokeApiOption)));
builder.Services.AddMongoDb(builder.Configuration.GetSection(nameof(MongoDbOption)));
// Add Gen I Worker
builder.Services.AddSingleton<IHostedService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Worker>>();
    var workerOption = provider.GetRequiredService<IOptions<WorkerOption>>();
    var pokeApiOption = provider.GetRequiredService<IOptions<PokeApiOption>>();
    var pokemonClient = provider.GetRequiredService<PokemonHttpClient>();
    var mongoDbService = provider.GetRequiredService<MongoDbService>();
    return new Worker(logger,
        workerOption.Value,
        pokeApiOption.Value,
        Enumerable.Range(1, 151),
        pokemonClient,
        mongoDbService);
});

var host = builder.Build();
host.Run();