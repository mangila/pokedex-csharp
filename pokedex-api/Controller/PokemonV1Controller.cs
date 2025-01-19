using System.Net.Mime;
using Asp.Versioning;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pokedex_shared.Config;
using pokedex_shared.Model;
using pokedex_shared.Service;

namespace pokedex_api.Controller;

[ApiVersion(1)]
[ApiController]
[Route("api/v{v:apiVersion}/pokemon")]
[Produces(MediaTypeNames.Application.Json)]
[EnableRateLimiting(HttpRateLimiterConfig.Policies.FixedWindow)]
public class PokemonV1Controller(
    ILogger<PokemonV1Controller> logger,
    PokemonService pokemonService)
    : ControllerBase
{
    /// <summary>
    /// Return all Pokémon as a list
    /// </summary>
    /// <param name="cancellationToken">Ignored by swagger</param>
    /// <response code="200">All Pokémons as a List</response>
    /// <response code="409">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindAll(CancellationToken cancellationToken)
    {
        var collection = await pokemonService.FindAllAsync(cancellationToken);
        return Results.Ok(collection);
    }

    /// <summary>
    /// Return Pokemon by id
    /// </summary>
    /// <param name="pokemonId"></param>
    /// <param name="cancellationToken">Ignored by swagger</param>
    /// <response code="200">Pokemon by Id</response>
    /// <response code="404">Not found</response>
    /// <response code="409">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet("{id:int}")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<IResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByPokemonId(
        [FromRoute] PokemonId pokemonId,
        CancellationToken cancellationToken
    )
    {
        var dto = await pokemonService.FindOneByPokemonIdAsync(pokemonId, cancellationToken);
        return dto.HasValue ? Results.Ok(dto) : Results.NotFound();
    }

    /// <summary>
    /// Return Pokemon by name
    /// </summary>
    /// <param name="pokemonName"></param>
    /// <param name="cancellationToken">Ignored by swagger</param>
    /// <response code="200">Pokemon by name</response>
    /// <response code="404">Not found</response>
    /// <response code="409">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet("{name}")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<IResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByPokemonName(
        [FromRoute] PokemonName pokemonName,
        CancellationToken cancellationToken
    )
    {
        var dto = await pokemonService.FindOneByNameAsync(pokemonName, cancellationToken);
        return dto.HasValue ? Results.Ok(dto) : Results.NotFound();
    }

    /// <summary>Search for pokemons by id</summary>
    /// <param name="ids">
    /// Array of ids
    /// </param>
    /// <param name="cancellationToken">Ignored by swagger</param>
    /// <remarks>
    /// Sample Ids:<br></br>
    /// * 1 <br></br>
    /// * 74 <br></br>
    /// * 66
    /// </remarks>
    /// <response code="200">Returns One Pokémon</response>
    /// <response code="400">Validation error</response>
    /// <response code="404">Pokémon not found</response>
    /// <response code="409">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet]
    [Route("search/id")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<IResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> QueryById(
        [FromQuery] List<int> ids,
        CancellationToken cancellationToken
    )
    {
        var collection = await pokemonService.FindAllByPokemonIdAsync(
            new PokemonIdCollection(ids),
            cancellationToken);
        return Results.Ok(collection);
    }

    /// <summary>
    /// Search for Pokemon by name
    /// </summary>
    /// <param name="search"></param>
    /// <param name="cancellationToken">Ignored by swagger</param>
    /// <remarks>
    /// Sample Names:<br></br>
    /// * charizard <br></br>
    /// * bulbasaur <br></br>
    /// * Mr.Mime
    /// </remarks>
    /// <response code="200">Returns One Pokémon</response>
    /// <response code="400">Validation error</response>
    /// <response code="404">Pokémon not found</response>
    /// <response code="409">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet]
    [Route("search/name")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> QueryByName(
        [FromQuery] string search,
        CancellationToken cancellationToken
    )
    {
        var collection = await pokemonService.SearchByName(search, cancellationToken);
        return Results.Ok(collection);
    }
}