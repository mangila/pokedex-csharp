using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pokedex_api.Config;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;
using pokedex_shared.Service;

namespace pokedex_api.Controller;

[ApiController]
[Route("api/v1/pokemon/search")]
[Produces(MediaTypeNames.Application.Json)]
[EnableRateLimiting(HttpRateLimiterConfig.Policies.FixedWindow)]
[RequestTimeout(HttpRequestConfig.Policies.ThreeMinute)]
public class PokemonV1SearchController(
    ILogger<PokemonV1SearchController> logger,
    PokemonService pokemonService)
    : ControllerBase
{
    [HttpGet("id")]
    [ProducesResponseType<ImmutableList<PokemonSpeciesDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> QueryById(
        [Required] [FromQuery] List<int> ids,
        CancellationToken cancellationToken = default
    )
    {
        var collection = await pokemonService.FindAllByIdsAsync(
            new PokemonIdCollection(ids),
            cancellationToken);
        return Results.Ok(collection);
    }

    [HttpGet("name")]
    [ProducesResponseType<ImmutableList<PokemonSpeciesDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> QueryByName(
        [Required] [FromQuery] string search,
        CancellationToken cancellationToken = default
    )
    {
        var collection = await pokemonService.SearchByNameAsync(
            new PokemonName(search),
            cancellationToken);
        return Results.Ok(collection);
    }

    [HttpGet("generation")]
    [ProducesResponseType<ImmutableList<PokemonSpeciesDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> QueryByGeneration(
        [Required] [FromQuery] string generation,
        CancellationToken cancellationToken = default
    )
    {
        var collection =
            await pokemonService.SearchByGenerationAsync(PokemonGeneration.From(generation),
                cancellationToken);
        return Results.Ok(collection);
    }
}