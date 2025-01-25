using System.Net.Mime;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pokedex_api.Config;
using pokedex_shared.Model.Domain;
using pokedex_shared.Service;
using pokedex_shared.Service.Query;

namespace pokedex_api.Controller;

[ApiController]
[Route("api/v1/pokemon/search")]
[Produces(MediaTypeNames.Application.Json)]
[EnableRateLimiting(HttpRateLimiterConfig.Policies.FixedWindow)]
public class PokemonV1SearchController(
    ILogger<PokemonV1SearchController> logger,
    PokemonQueryService pokemonQueryService)
    : ControllerBase
{
    [HttpGet("id")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    public async Task<IResult> QueryById(
        [FromQuery] List<int> ids,
        CancellationToken cancellationToken = default
    )
    {
        var collection = await pokemonQueryService.FindAllByPokemonIdAsync(
            new PokemonIdCollection(ids),
            cancellationToken);
        return Results.Ok(collection);
    }

    [HttpGet("name")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    public async Task<IResult> QueryByName(
        [FromQuery] string search,
        CancellationToken cancellationToken = default
    )
    {
        var collection = await pokemonQueryService.SearchByNameAsync(new PokemonName(search), cancellationToken);
        return Results.Ok(collection);
    }
}