using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using pokedex_shared.Config;

namespace pokedex_shared.Extension;

public static class Extensions
{
    public static void Validate<T>(this T type)
    {
        if (type is not object variable) throw new ArgumentException("type is not object");
        var context = new ValidationContext(type, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(type, context, results, validateAllProperties: true);
        if (results.Count != 0)
        {
            throw new ValidationException(string.Join(", ", results));
        }
    }

    public static async Task<string> ToJsonAsync<T>(this T type, CancellationToken cancellationToken = default)
    {
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, type, JsonConfig.JsonOptions, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position to the beginning
        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync(cancellationToken);
    }
}