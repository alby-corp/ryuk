using Atlassian.Jira;
using MudBlazor;
using Ryuk.Extensions;

namespace Ryuk.Components.Pages.MySpace.Grids;

using Severity = Model.Severity;

public class ToDoModel(Issue issue)
{
    public string Key { get; } = issue?.Key.Value ?? string.Empty;
    public List<BreadcrumbItem> Breadcrumbs { get; } = issue?.ToBreadcrumbs() ?? [];

    public string Status { get; } = issue?.Status.Name ?? string.Empty;
    public Color StatusColor { get; } = issue?.StatusColor() ?? Color.Transparent;

    public string Type { get; } = issue?.Type.Name ?? string.Empty;
    public string TypeIconUrl { get; } = issue?.Type.IconUrl ?? string.Empty;

    public string Summary { get; } = issue?.Summary ?? string.Empty;

    public DateTime? StartDate { get; set; } = issue?.StartDate();
    public DateTime? DueDate { get; set; } = issue?.DueDate;

    public string OriginalEstimate { get; set; } = issue?.TimeTrackingData.OriginalEstimate ?? string.Empty;

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

        if (issue.StartDate() > DateTime.Now.AddDays(1) && issue.StartDate() < DateTime.Now)
            yield return new(Severity.Warning, Color.Warning, "Caution! The start date is approaching the expected workday.");

        if (issue.StartDate() > DateTime.Now)
            yield return new(Severity.Error, Color.Error, "Oh no! The start of work is delayed. Urgent intervention required!");
    }
}