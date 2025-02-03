using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using pokedex_shared.Config;

namespace pokedex_shared.Extension;

public static class ExtensionMapper
{
    public static void Validate<T>(this T type) where T : struct
    {
        object obj = type;
        var context = new ValidationContext(obj);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(obj, context, results, validateAllProperties: true);
        if (results.Count != 0)
        {
            throw new ValidationException(string.Join(", ", results));
        }
    }

    public static async Task<T> DeserializeJsonAsync<T>(this string json,
        CancellationToken cancellationToken = default) where T : struct
    {
        var bytes = Encoding.UTF8.GetBytes(json);
        await using var memoryStream = new MemoryStream(bytes);
        return await JsonSerializer.DeserializeAsync<T>(memoryStream, JsonConfig.JsonOptions, cancellationToken);
    }

    public static async Task<string> ToJsonValueTypeAsync<T>(this T type,
        CancellationToken cancellationToken = default) where T : struct
    {
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, type, JsonConfig.JsonOptions, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position to the beginning
        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    public static async Task<string> ToJsonReferenceTypeAsync<T>(this T type,
        CancellationToken cancellationToken = default) where T : class
    {
        await using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, type, JsonConfig.JsonOptions, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position to the beginning
        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync(cancellationToken);
    }
}