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
    .Bind(builder.Configuration.GetSection(nameof(WorkerOption)))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<PokedexApiOption>()
    .Bind(builder.Configuration.GetSection(nameof(PokedexApiOption)))
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
builder.Services.AddPokemonApi(builder.Configuration.GetSection(nameof(PokeApiOption)));
builder.Services.AddMongoDbCommandService(builder.Configuration.GetSection(nameof(MongoDbOption)));
builder.Services.AddSingleton<RedisService>();

// Add GenerationI Worker
builder.Services.AddSingleton<IHostedService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Worker>>();
    var workerOption = provider.GetRequiredService<IOptions<WorkerOption>>();
    var pokeApiOption = provider.GetRequiredService<IOptions<PokeApiOption>>();
    var pokemonClient = provider.GetRequiredService<PokemonHttpClient>();
    var mongoDbService = provider.GetRequiredService<MongoDbCommandService>();
    var mongoDbGridFsService = provider.GetRequiredService<MongoDbGridFsCommandService>();
    return new Worker(logger,
        workerOption.Value,
        pokeApiOption.Value,
        PokemonGeneration.GenerationI,
        pokemonClient,
        mongoDbService,
        mongoDbGridFsService);
});
// Add GenerationII Worker
builder.Services.AddSingleton<IHostedService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Worker>>();
    var workerOption = provider.GetRequiredService<IOptions<WorkerOption>>();
    var pokeApiOption = provider.GetRequiredService<IOptions<PokeApiOption>>();
    var pokemonClient = provider.GetRequiredService<PokemonHttpClient>();
    var mongoDbService = provider.GetRequiredService<MongoDbCommandService>();
    var mongoDbGridFsService = provider.GetRequiredService<MongoDbGridFsCommandService>();
    return new Worker(logger,
        workerOption.Value,
        pokeApiOption.Value,
        PokemonGeneration.GenerationII,
        pokemonClient,
        mongoDbService,
        mongoDbGridFsService);
});

var host = builder.Build();
host.Run();