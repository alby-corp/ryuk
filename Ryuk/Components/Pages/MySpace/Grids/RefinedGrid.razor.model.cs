namespace Ryuk.Components.Pages.MySpace.Grids;

using Atlassian.Jira;
using Extensions;
using MudBlazor;
using Error = Model.Error;
using IssueChangeLogItem = Model.IssueChangeLogItem;
using Severity = Model.Severity;

public class RefinedModel(Issue issue, IEnumerable<IssueChangeLogItem> issueChangeLogItems)
{

    public string Key { get; } = issue?.Key.Value ?? string.Empty;
    public List<BreadcrumbItem> Breadcrumbs { get; } = issue?.ToBreadcrumbs() ?? [];

    public string? Status { get; } = issue?.Status.Name;
    public Color StatusColor { get; } = issue?.StatusColor() ?? Color.Transparent;

    public string Type { get; } = issue?.Type.Name ?? string.Empty;
    public string? TypeIconUrl { get; } = issue?.Type.IconUrl ?? string.Empty;

    public string Summary { get; } = issue?.Summary ?? string.Empty;

    public DateTime? StartDate { get; set; } = issue?.StartDate();
    public DateTime? DueDate { get; set; } = issue?.DueDate;

    public string OriginalEstimate { get; set; } = issue?.TimeTrackingData.OriginalEstimate ?? string.Empty;
    public long OriginalEstimateInSeconds { get; set; } = issue?.TimeTrackingData.OriginalEstimateInSeconds ?? 0;
    public IEnumerable<Error> Errors { get; } = Validate(issue, issueChangeLogItems);
    public bool HasErrors => Errors.Any(error => error.Severity == Severity.Error);

    static IEnumerable<Error> Validate(Issue? issue, IEnumerable<IssueChangeLogItem>? issueChangeLogs)
    {
        var lastStatusChange = (issueChangeLogs ?? Enumerable.Empty<IssueChangeLogItem>())
            .Where(x => (x.LogItem.FieldName == "status") & (x.LogItem.ToValue == Model.Status.Refined))
            .MaxBy(x => x.CreatedDate)
            ?.CreatedDate;

        if (issue?.DueDate is null)
            yield return new(Severity.Error, Color.Error, "Missing Due Date");

        if (issue?.StartDate() is null)
            yield return new(Severity.Error, Color.Error, "Missing Start Date");

        if (string.IsNullOrEmpty(issue?.TimeTrackingData.OriginalEstimate) ||
            issue.TimeTrackingData.OriginalEstimateInSeconds < 1)
            yield return new(Severity.Error, Color.Error, "Missing Original Estimate");

        if (!string.IsNullOrEmpty(issue?.TimeTrackingData.OriginalEstimate) && issue.StartDate() is not null &&
            issue.DueDate is not null)
            yield return new(Severity.Success, Color.Success, "Issue is in the wrong state! Move it to ToDo");

        if (lastStatusChange < DateTime.Now)
            yield return new(Severity.Warning, Color.Warning, "Issue in the refined state for more than 1 day");
    }
}