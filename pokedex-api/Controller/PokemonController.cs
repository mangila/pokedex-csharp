using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Asp.Versioning;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using pokedex_api.Model;
using pokedex_api.Service;
using static pokedex_api.Config.HttpConfig;

namespace pokedex_api.Controller;

[ApiVersion(1)]
[ApiController]
[Route("api/v{v:apiVersion}/pokemons")]
[Produces(MediaTypeNames.Application.Json)]
[EnableRateLimiting(RateLimitPolicies.FixedWindowPolicy)]
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
    [RequestTimeout(RequestPolicies.OneSecondPolicy)]
    [ProducesResponseType<IEnumerable<PokemonResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<IEnumerable<PokemonResponse>>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IEnumerable<PokemonResponse>> FindAll(
        [FromQuery] List<string> ids,
        CancellationToken cancellationToken
    )
    {
        if (ids.Count == 0)
        {
            return pokemonService.FindAllAsync(cancellationToken);
        }

        return pokemonService.FindAllByIdAsync(ids, cancellationToken);
    }

    /// <summary>
    /// Find one Pokémon by name or id
    /// </summary>
    /// <param name="value">
    /// Search for ID or Name
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
    [Route("search")]
    [RequestTimeout(RequestPolicies.FiveHundredMsSecondPolicy)]
    [ProducesResponseType<PokemonResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindByQuery(
        [FromQuery]
        [Required]
        [StringLength(200,
            ErrorMessage = "\"value\" query param cannot be over 200 characters")]
        [RegularExpression("[A-Za-z\\d]+",
            ErrorMessage = "\"value\" query param must be a number or letter!")]
        string value,
        CancellationToken cancellationToken
    )
    {
        var pokemonResponse = pokemonService.FindOneByAsync(value, cancellationToken);
        if (pokemonResponse is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(pokemonResponse);
    }
}