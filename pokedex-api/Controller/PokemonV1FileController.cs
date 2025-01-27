using System.Net.Mime;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Bson;
using pokedex_api.Config;
using pokedex_shared.Service.Query;

namespace pokedex_api.Controller;

[ApiController]
[Route("api/v1/pokemon/file")]
[Produces(MediaTypeNames.Application.Octet)]
[EnableRateLimiting(HttpRateLimiterConfig.Policies.FixedWindow)]
[RequestTimeout(HttpRequestConfig.Policies.OneMinute)]
public class PokemonV1FileController(
    ILogger<PokemonV1FileController> logger,
    PokemonQueryService pokemonQueryService)
    : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IResult> FindFileById(
        [FromRoute] ObjectId id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await pokemonQueryService.FindFileByIdAsync(id, cancellationToken);
        if (result.HasValue)
        {
            var file = result.Value;
            Response.Headers.CacheControl = "public, max-age=43200";
            return Results.File(file.File, file.ContentType, file.FileName);
        }

        return Results.NotFound();
    }
}