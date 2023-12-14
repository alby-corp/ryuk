namespace Ryuk.Components.Pages.MySpace.Grids;

using System.ComponentModel.DataAnnotations;
using Atlassian.Jira;
using Extensions;
using Model;
using MudBlazor;
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

    public DateTime? StartDate { get; } = issue?.StartDate();
    public DateTime? DueDate { get; } = issue?.DueDate;

    public string OriginalEstimate { get; } = issue?.TimeTrackingData.OriginalEstimate ?? string.Empty;
    public long OriginalEstimateInSeconds { get; } = issue?.TimeTrackingData.OriginalEstimateInSeconds ?? 0;

    public string TimeSpent { get; } = issue?.TimeTrackingData.TimeSpent ?? string.Empty;
    public long TimeSpentInSeconds { get; } = issue?.TimeTrackingData.TimeSpentInSeconds ?? 0;

    [RegularExpression(@"^ *([0-9]+[WwDdHhMm])( +[0-9]+[WwDdHhMm])* *$")]
    public string RemainingEstimate { get; set; } = issue?.TimeTrackingData.RemainingEstimate ?? string.Empty;

    public long RemainingEstimateInSeconds { get; } = issue?.TimeTrackingData.RemainingEstimateInSeconds ?? 0;

    public IEnumerable<Error> Errors { get; } = Validate(issue);
    public bool HasErrors => Errors.Any(error => error.Severity == Severity.Error);

    static IEnumerable<Error> Validate(Issue? issue)
    {
        if (issue is null) yield break;

        if (issue.DueDate is null)
            yield return new(Severity.Error, Color.Error, "Missing Due Date");

        if (string.IsNullOrEmpty(issue.TimeTrackingData.OriginalEstimate) ||
            issue.TimeTrackingData.OriginalEstimateInSeconds < 1)
            yield return new(Severity.Error, Color.Error, "Missing Original Estimate");

        if (issue.StartDate() is null)
            yield return new(Severity.Error, Color.Error, "Missing Start Date");

        if (issue.TimeTrackingData.TimeSpentInSeconds > issue.TimeTrackingData.OriginalEstimateInSeconds)
            yield return new(Severity.Warning, Color.Warning, "Time Spent on this issue exceed Original Estimate");

        if (issue.TimeTrackingData.TimeSpentInSeconds == issue.TimeTrackingData.OriginalEstimateInSeconds)
            yield return new(Severity.Warning, Color.Warning, "There is no more time to spend on this issue");
    }
}