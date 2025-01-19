using System.Net.Mime;
using Asp.Versioning;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pokedex_api.Config;
using pokedex_shared.Model.Domain;
using pokedex_shared.Model.Dto;
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
    /// <response code="200">All Pokémons as a List</response>
    /// <response code="400">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindAll(CancellationToken cancellationToken = default)
    {
        var collection = await pokemonService.FindAllAsync(cancellationToken);
        return Results.Ok(collection);
    }

    /// <summary>
    /// Return Pokemon by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Pokemon by Id</response>
    /// <response code="400">Something went wrong with the request</response>
    /// <response code="404">Not found</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet("{id:int}")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<IResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByPokemonId(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        var dto = await pokemonService.FindOneByPokemonIdAsync(new PokemonId(id.ToString()), cancellationToken);
        return dto.HasValue ? Results.Ok(dto) : Results.NotFound();
    }

    /// <summary>
    /// Return Pokemon by name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Pokemon by name</response>
    /// <response code="400">Something went wrong with the request</response>
    /// <response code="404">Not found</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet("{name}")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<IResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByPokemonName(
        [FromRoute] string name,
        CancellationToken cancellationToken = default
    )
    {
        var dto = await pokemonService.FindOneByNameAsync(new PokemonName(name), cancellationToken);
        return dto.HasValue ? Results.Ok(dto) : Results.NotFound();
    }

    /// <summary>Search for pokemons by id</summary>
    /// <param name="ids">
    /// Array of ids
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// Sample Ids:<br></br>
    /// * 1 <br></br>
    /// * 74 <br></br>
    /// * 66
    /// </remarks>
    /// <response code="200">Returns One Pokémon</response>
    /// <response code="400">Something went wrong the the request</response>
    /// <response code="404">Pokémon not found</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet]
    [Route("search/id")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<IResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> QueryById(
        [FromQuery] List<int> ids,
        CancellationToken cancellationToken = default
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
    /// <param name="cancellationToken"></param>
    /// <remarks>
    /// Sample Names:<br></br>
    /// * charizard <br></br>
    /// * bulbasaur <br></br>
    /// * Mr.Mime
    /// </remarks>
    /// <response code="200">Returns list of Pokemons with search result</response>
    /// <response code="400">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet]
    [Route("search/name")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<PokemonDtoCollection>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> QueryByName(
        [FromQuery] string search,
        CancellationToken cancellationToken = default
    )
    {
        var collection = await pokemonService.SearchByName(search, cancellationToken);
        return Results.Ok(collection);
    }
}