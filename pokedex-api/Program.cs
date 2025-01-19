using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using pokedex_api.ExceptionHandler;
using pokedex_shared.Config;
using pokedex_shared.Option;
using pokedex_shared.Service;

var builder = WebApplication.CreateBuilder(args);

// Configure API Controllers
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Pokedex API",
        Description = "An .NET Core Restful API with Pokémon´s",
        Contact = new OpenApiContact
        {
            Name = "Mangila",
            Url = new Uri("https://github.com/mangila")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License"
        }
    });
    // Set the XML-comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
// Configure MongoDB
builder.Services.AddMongoDb(builder.Configuration.GetSection(nameof(MongoDbOption)));
// Configure Redis
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;
});
// Configure Request timeouts
builder.Services.AddRequestTimeouts(HttpRequestConfig.ConfigureRequestTimeout);
// Configure Rate limiting
builder.Services.AddRateLimiter(HttpRateLimiterConfig.ConfigureRateLimiter);
// Configure Default [ApiController] behaviour
builder.Services.Configure<ApiBehaviorOptions>(ApiBehaviourConfig.ConfigureApiBehaviourOptions);
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version"));
    })
    .AddMvc()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });
// Add Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// Add Singleton Services
builder.Services.AddSingleton<PokemonService>();
builder.Services.AddSingleton<DatasourceService>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "Swagger UI - Pokedex API";
    options.DisplayRequestDuration();
});
app.UseRequestTimeouts();
app.UseRateLimiter();
app.MapControllers();
app.Run();