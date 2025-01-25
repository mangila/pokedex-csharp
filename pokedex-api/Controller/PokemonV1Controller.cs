using System.Net.Mime;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Bson;
using pokedex_api.Config;
using pokedex_shared.Model.Domain;
using pokedex_shared.Service;
using pokedex_shared.Service.Query;

namespace pokedex_api.Controller;

[ApiController]
[Route("api/v1/pokemon")]
[Produces(MediaTypeNames.Application.Json)]
[EnableRateLimiting(HttpRateLimiterConfig.Policies.FixedWindow)]
public class PokemonV1Controller(
    ILogger<PokemonV1Controller> logger,
    PokemonQueryService pokemonQueryService)
    : ControllerBase
{
    [HttpGet]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    public async Task<IResult> FindAll(CancellationToken cancellationToken = default)
    {
        var collection = await pokemonQueryService.FindAllAsync(cancellationToken);
        return Results.Ok(collection);
    }

    [HttpGet("{id:int}")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    public async Task<IResult> FindByPokemonId(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        var dto = await pokemonQueryService.FindOneByPokemonIdAsync(new PokemonId(id.ToString()), cancellationToken);
        return dto.HasValue ? Results.Ok(dto) : Results.NotFound();
    }

    [HttpGet("{name}")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    public async Task<IResult> FindByPokemonName(
        [FromRoute] string name,
        CancellationToken cancellationToken = default
    )
    {
        var dto = await pokemonQueryService.FindOneByNameAsync(new PokemonName(name), cancellationToken);
        return dto.HasValue ? Results.Ok(dto) : Results.NotFound();
    }
}