using Atlassian.Jira;
using MudBlazor;
using Ryuk.Extensions;

namespace Ryuk.Components.Pages.MySpace.Grids;

public class RefinementModel(Issue? issue)
{
    public RefinementModel() : this(default)
    {
    }

    public string Key { get; } = issue?.Key.Value ?? string.Empty;
    public List<BreadcrumbItem> Breadcrumbs { get; } = issue?.ToBreadcrumbs() ?? [];

    public string? Status { get; } = issue?.Status.Name;
    public Color StatusColor { get; } = issue?.StatusColor() ?? Color.Transparent;

    public string? Type { get; } = issue?.Type.Name ?? string.Empty;
    public string TypeIconUrl { get; } = issue?.Type.IconUrl ?? string.Empty;

    public string Summary { get; } = issue?.Summary ?? string.Empty;
    public string OriginalEstimate { get; set; } = issue?.TimeTrackingData.OriginalEstimate ?? string.Empty;
    
    public long OriginalEstimateInSeconds { get; set; } = issue?.TimeTrackingData.OriginalEstimateInSeconds ?? 0;
}