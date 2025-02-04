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
[Route("api/v1/pokemon")]
[Produces(MediaTypeNames.Application.Json)]
[EnableRateLimiting(HttpRateLimiterConfig.Policies.FixedWindow)]
[RequestTimeout(HttpRequestConfig.Policies.ThreeMinute)]
public class PokemonV1Controller(
    ILogger<PokemonV1Controller> logger,
    PokemonService pokemonService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<PaginationResultDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByPagination(
        [Required] [FromQuery] [Range(0, int.MaxValue, ErrorMessage = "The Page must be a non-negative integer.")]
        int page,
        [Required]
        [FromQuery]
        [Range(0, 100, ErrorMessage = "The Page size must be a non-negative integer and in the range 1-100")]
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var collection = await pokemonService.FindByPaginationAsync(
            page,
            pageSize,
            cancellationToken);
        return Results.Ok(collection);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<PokemonSpeciesDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindById(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        var dto = await pokemonService.FindOneByIdAsync(
            new PokemonId(id),
            cancellationToken);
        return dto.Equals(default) ? Results.NotFound() : Results.Ok(dto);
    }

    [HttpGet("{name}")]
    [ProducesResponseType<PokemonSpeciesDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByPokemonName(
        [FromRoute] string name,
        CancellationToken cancellationToken = default
    )
    {
        var dto = await pokemonService.FindOneByNameAsync(
            new PokemonName(name),
            cancellationToken);
        return dto.Equals(default) ? Results.NotFound() : Results.Ok(dto);
    }
}