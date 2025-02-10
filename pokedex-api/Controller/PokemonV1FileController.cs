using System.Net.Mime;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using pokedex_api.Config;
using pokedex_shared.Service;

namespace pokedex_api.Controller;

[ApiController]
[Route("api/v1/pokemon/file")]
[Produces(MediaTypeNames.Application.Octet)]
[RequestTimeout(HttpRequestConfig.Policies.ThreeMinute)]
public class PokemonV1FileController(PokemonService pokemonService) : ControllerBase
{
    [HttpGet("{id}")]
    [ProducesResponseType<FileResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> FindFileById(
        [FromRoute] ObjectId id,
        CancellationToken cancellationToken = default
    )
    {
        var result = await pokemonService.FindFileByIdAsync(id, cancellationToken);
        if (result.HasValue)
        {
            var file = result.Value;
            return Results.File(
                fileContents: file.File,
                contentType: file.ContentType,
                fileDownloadName: file.FileName,
                lastModified: file.LastModified
            );
        }

        return Results.NotFound();
    }
}