namespace Ryuk.Extensions;

using System.Globalization;

public static class DateTimeExtensions
{
    public static string ToLocalDateOnly(this DateTime? datetime) => datetime.HasValue
        ? datetime.Value.ToString("d", CultureInfo.CurrentUICulture)
        : string.Empty;
}