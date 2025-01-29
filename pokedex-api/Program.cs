using Microsoft.AspNetCore.Mvc;
using pokedex_api.Config;
using pokedex_api.ExceptionHandler;
using pokedex_shared.Config;
using pokedex_shared.Option;
using pokedex_shared.Service;
using pokedex_shared.Service.Query;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure API Controllers
builder.Services.AddControllers();
// Configure MongoDB
builder.Services.AddMongoDbQueryService(builder.Configuration.GetRequiredSection(nameof(MongoDbOption)));
// Configure Redis
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;
    redisOptions.InstanceName = "pokedex-api:development:";
});
// Configure Request timeouts
builder.Services.AddRequestTimeouts(HttpRequestConfig.ConfigureRequestTimeout);
// Configure Rate limiting
builder.Services.AddRateLimiter(HttpRateLimiterConfig.ConfigureRateLimiter);
// Configure Default [ApiController] behaviour
builder.Services.Configure<ApiBehaviorOptions>(ApiBehaviourConfig.ConfigureApiBehaviourOptions);
// Load Serilog from configuration
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
// Add Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// Add Singleton Services
builder.Services.AddSingleton<PokemonQueryService>();
builder.Services.AddSingleton<RedisService>();
builder.Services.AddSingleton<DatasourceQueryService>();

var app = builder.Build();
app.UseCors(policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
app.UseExceptionHandler();
app.UseSerilogRequestLogging();
app.UseRequestTimeouts();
app.UseRateLimiter();
app.MapControllers();
app.Run();