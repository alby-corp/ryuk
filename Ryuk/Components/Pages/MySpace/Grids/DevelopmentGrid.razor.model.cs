using Atlassian.Jira;
using MudBlazor;
using Ryuk.Extensions;

namespace Ryuk.Components.Pages.MySpace.Grids;

using Severity = Model.Severity;

public class DevelopmentModel(Issue? issue)
{
    public DevelopmentModel() : this(default)
    {
    }

    public string Key { get; } = issue?.Key.Value ?? string.Empty;
    public List<BreadcrumbItem> Breadcrumbs { get; } = issue?.ToBreadcrumbs() ?? [];

    public string? Status { get; } = issue?.Status.Name;
    public Color StatusColor { get; } = issue?.StatusColor() ?? Color.Transparent;

    public string Type { get; } = issue?.Type.Name ?? string.Empty;
    public string TypeIconUrl { get; } = issue?.Type.IconUrl ?? string.Empty;

    public string Summary { get; } = issue?.Summary ?? string.Empty;

    public DateTime? StartDate { get; set; } = issue?.StartDate();
    public DateTime? DueDate { get; set; } = issue?.DueDate;

    public string OriginalEstimate { get; set; } = issue?.TimeTrackingData.OriginalEstimate ?? string.Empty;
    public long OriginalEstimateInSeconds { get; set; } = issue?.TimeTrackingData.OriginalEstimateInSeconds ?? 0;

    public string TimeSpent { get; set; } = issue?.TimeTrackingData.TimeSpent ?? string.Empty;
    public long TimeSpentInSeconds { get; set; } = issue?.TimeTrackingData.TimeSpentInSeconds ?? 0;

    public string RemainingEstimate { get; set; } = issue?.TimeTrackingData.RemainingEstimate ?? string.Empty;
    public long RemainingEstimateInSeconds { get; set; } = issue?.TimeTrackingData.RemainingEstimateInSeconds ?? 0;

    public IEnumerable<Model.Error> Errors { get; } = Validate(issue);
    public bool HasErrors => Errors.Any(error => error.Severity == Severity.Error);

    static IEnumerable<Model.Error> Validate(Issue? issue)
    {
        if (issue is null) yield break;

        if (issue.DueDate is null)
            yield return new(Severity.Error, Color.Error, "Missing Due Date");

        if (string.IsNullOrEmpty(issue.TimeTrackingData.OriginalEstimate) || issue.TimeTrackingData.OriginalEstimateInSeconds < 1)
            yield return new(Severity.Error, Color.Error, "Missing Original Estimate");

        if (!string.IsNullOrEmpty(issue.TimeTrackingData.OriginalEstimate) && issue.StartDate() is not null && issue.DueDate is not null)
            yield return new(Severity.Success, Color.Success, "Issue is in the wrong state! Move it to ToDo");

        if (issue.StartDate() is null)
            yield return new(Severity.Error, Color.Error, "Missing Start Date");

        if (issue.Created < DateTime.Now.AddDays(-1) && issue.Created > DateTime.Now.AddDays(-2))
            yield return new(Severity.Warning, Color.Warning, "Issue in the refined state for more than 1 day");

        if (issue.Created < DateTime.Now.AddDays(-2))
            yield return new(Severity.Warning, Color.Warning, "Issue in the refined state for more than 2 days");
    }
}