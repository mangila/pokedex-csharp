using Microsoft.Extensions.Options;
using pokedex_poller;
using pokedex_poller.Config;
using pokedex_shared.Http;

var builder = Host.CreateApplicationBuilder(args);
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
// Add Gen I
builder.Services.AddSingleton<IHostedService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<Worker>>();
    var workerOption = provider.GetRequiredService<IOptions<WorkerOption>>();
    var pokemonClient = provider.GetRequiredService<PokemonClient>();
    return new Worker(logger, workerOption.Value, Enumerable.Range(1, 151), pokemonClient);
});

var host = builder.Build();
host.Run();