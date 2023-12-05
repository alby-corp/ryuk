using Atlassian.Jira;
using MudBlazor;
using Ryuk.Extensions;

namespace Ryuk.Components.Pages.MySpace.Grids;

public class RefinementModel(Issue issue)
{
    public string Key { get; } = issue.Key.Value;
    public List<BreadcrumbItem> Breadcrumbs { get; } = issue.ToBreadcrumbs();

    public string Status { get; } = issue.Status.Name;
    public Color StatusColor { get; } = issue.StatusColor();

    public string Type { get; } = issue.Type.Name;
    public string TypeIconUrl { get; } = issue.Type.IconUrl;

    public string Summary { get; } = issue.Summary;
}