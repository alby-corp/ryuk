namespace Ryuk.Extensions;

using System.Text.Json;

public static class ExceptionExtensions
{
    public static Dictionary<string, string> ExtractErrors(this Exception exception)
    {
        try
        {
            if (string.IsNullOrEmpty(exception.Message)) return new();

            var jsonContentIndex = exception.Message.IndexOf("{", StringComparison.Ordinal);
            if (jsonContentIndex < 0) return new();

            var jsonContent = exception.Message[jsonContentIndex..];

            using var doc = JsonDocument.Parse(jsonContent);

            return doc.RootElement.TryGetProperty("errors", out var errorsElement) && errorsElement.ValueKind == JsonValueKind.Object
                ? errorsElement
                    .EnumerateObject()
                    .ToDictionary(e => e.Name, property => $"{property.Value}")
                : new();
        }
        catch
        {
            return new() { { "Error", exception.Message } };
        }
    }
}