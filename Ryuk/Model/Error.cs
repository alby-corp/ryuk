namespace Ryuk.Model;

using MudBlazor;

public record Error(Severity Severity, Color Color, string Message)
{
    readonly string _color = Color.ToString().ToLower();

    public string ThemeClass => $"mud-theme-{_color}";
    public string ColorClass => $"mud-{_color}-text";
    public string BackgroundClass => $"mud-{_color}";

    public string Icon =>
        Severity switch
        {
            Severity.Error => Icons.Material.Outlined.PriorityHigh,
            Severity.Warning => Icons.Material.Outlined.WarningAmber,
            Severity.Success => Icons.Material.Outlined.FileDownloadDone,
            _ => Icons.Material.Outlined.Bookmark
        };
}