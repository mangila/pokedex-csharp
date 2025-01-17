using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace pokedex_shared.Config;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
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
        return services;
    }
}