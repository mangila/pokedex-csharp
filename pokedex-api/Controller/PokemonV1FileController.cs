using System.Net.Mime;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Bson;
using pokedex_api.Config;
using pokedex_shared.Service;
using pokedex_shared.Service.Query;

namespace pokedex_api.Controller;

[ApiController]
[Route("api/v1/pokemon/file")]
[Produces(MediaTypeNames.Application.Octet)]
[EnableRateLimiting(HttpRateLimiterConfig.Policies.FixedWindow)]
public class PokemonV1FileController(
    ILogger<PokemonV1FileController> logger,
    PokemonQueryService pokemonQueryService)
    : ControllerBase
{
    [HttpGet("{id}")]
    [RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
    public async Task<IResult> FindFileById(
        [FromRoute] string id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await pokemonQueryService.FindFileByIdAsync(new ObjectId(id), cancellationToken);
        return result is not null ? Results.File(result.File, result.ContentType, result.FileName) : Results.NotFound();
    }
}