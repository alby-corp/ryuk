namespace Ryuk.Extensions;

using Model;
using MudBlazor;

public static class ErrorExtensions
{
    public static Color SeverityColor(this IEnumerable<Error> errors) => errors.MinBy(error => error.Severity)?.Color ?? Color.Info;

    public static string SeverityThemeClass(this IEnumerable<Error> errors) => errors.MinBy(error => error.Severity)?.ThemeClass ?? string.Empty;
    public static string SeverityColorClass(this IEnumerable<Error> errors) => errors.MinBy(error => error.Severity)?.ColorClass ?? string.Empty;
    public static string SeverityBackgroundColorClass(this IEnumerable<Error> errors) => errors.MinBy(error => error.Severity)?.BackgroundClass ?? string.Empty;
}