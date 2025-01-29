using pokedex_shared.Model.Dto.Collection;

namespace pokedex_shared.Model.Document;

public readonly record struct PaginationResultDocument(
    long TotalCount,
    int TotalPages,
    int CurrentPage,
    int PageSize,
    List<PokemonDocument> Documents
);

public static partial class Extensions
{
    public static PaginationResultDto ToDto(this PaginationResultDocument document)
    {
        return new PaginationResultDto(
            TotalCount: document.TotalCount,
            TotalPages: document.TotalPages,
            CurrentPage: document.CurrentPage,
            PageSize: document.PageSize,
            Documents: document.Documents.ToDtos()
        );
    }
}