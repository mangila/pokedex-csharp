namespace pokedex_shared.Model.Domain;

public readonly record struct PokemonMediaEntry(
    PokemonName Name,
    Uri Uri,
    string Description
);

public static class Extensions
{
    public static string GetContentType(this PokemonMediaEntry entry)
    {
        return entry.GetFileExtension() switch
        {
            "png" => "image/png",
            "ogg" => "audio/ogg",
            "svg" => "image/svg+xml",
            "gif" => "image/gif",
            _ => throw new NotSupportedException("content type not supported: " + entry.GetFileExtension())
        };
    }

    public static string GetFileExtension(this PokemonMediaEntry entry)
    {
        var indexOf = entry.Uri.OriginalString.LastIndexOf('.');
        return entry.Uri.OriginalString[(indexOf + 1)..];
    }

    public static string GetFileName(this PokemonMediaEntry entry)
    {
        var name = entry.Name.Value;
        var description = entry.Description;
        var fileExtension = entry.GetFileExtension();
        return $"{name}-{description}.{fileExtension}";
    }
}