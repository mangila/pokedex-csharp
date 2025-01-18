using System.ComponentModel.DataAnnotations;
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
public class PokemonController(
    ILogger<PokemonController> logger,
    PokemonService pokemonService)
    : ControllerBase
{
    /// <summary>
    /// Return all Pokémon as a list
    /// </summary>
    /// <param name="cancellationToken">Ignored by swagger. Http Request Token</param>
    /// <param name="ids">
    /// Search for IDs
    /// </param>
    /// <remarks>
    /// NOTE: If the query param is not defined endpoint will return all Pokémon<br></br>
    /// Sample Ids:<br></br>
    /// * 44 <br></br>
    /// * 150 <br></br>
    /// * 3
    /// </remarks>
    /// <response code="200">All Pokémons as a List</response>
    /// <response code="204">Empty List Response</response>
    /// <response code="409">Something went wrong with the request</response>
    /// <response code="500">Mangila messed up...</response>
    [MapToApiVersion(1)]
    [HttpGet]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    [ProducesResponseType<IEnumerable<PokemonDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<IEnumerable<PokemonDto>>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IEnumerable<PokemonDto>> FindAll(
        [FromQuery] List<int> ids,
        CancellationToken cancellationToken
    )
    {
        return ids.Count == 0
            ? pokemonService.FindAllAsync(cancellationToken)
            : pokemonService.FindAllByIdAsync(ids, cancellationToken);
    }

    /// <summary>
    /// Find one Pokémon by name or id
    /// </summary>
    /// <param name="value">
    /// Search for ID or Name
    /// </param>
    /// <param name="id">
    /// Search for id
    /// </param>
    /// <param name="cancellationToken">Ignored by swagger. Http Request Token</param>
    /// <remarks>
    /// Sample names:<br></br>
    /// * charizard<br></br>
    /// * bulbasaur<br></br>
    /// * Mr.Mime<br></br>
    /// 
    /// Sample Ids:<br></br>
    /// * 44 <br></br>
    /// * 150 <br></br>
    /// * 3
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
    [ProducesResponseType<PokemonDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<IResult>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByQuery(
        [FromQuery]
        [Required]
        [RegularExpression("[\\d]+",
            ErrorMessage = "\"id\" query param must be a number!")]
        int id,
        CancellationToken cancellationToken
    )
    {
        var pokemonResponse = await pokemonService.FindOneByIdAsync(id, cancellationToken);
        return pokemonResponse.HasValue ? Results.Ok(pokemonResponse.Value) : Results.NotFound();
    }
}