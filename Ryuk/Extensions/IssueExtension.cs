using Atlassian.Jira;
using MudBlazor;
using MudBlazor.Extensions;
using Ryuk.Model;

namespace Ryuk.Extensions;

public static class IssueExtension
{
    public static Issue GetByKey(this IEnumerable<Issue> issues, string key)
    {
        return issues.Single(issue => string.Equals(issue.Key.Value, key, StringComparison.InvariantCultureIgnoreCase));
    }

    public static bool InStatus(this Issue issue, params string[] statuses)
    {
        return statuses.Contains(issue.Status.Name, StringComparer.InvariantCultureIgnoreCase);
    }

    public static bool NotInStatus(this Issue issue, params string[] statuses)
    {
        return !statuses.Contains(issue.Status.Name, StringComparer.InvariantCultureIgnoreCase);
    }

    public static Color StatusColor(this Issue issue)
    {
        return issue.Status.Name switch
        {
            Status.Backlog => Color.Dark,
            Status.Refinement => Color.Dark,
            Status.Refined => Color.Warning,
            Status.Todo => Color.Info,
            Status.Blocked => Color.Dark,
            Status.InProgress => Color.Primary,
            Status.InternalReview => Color.Primary,
            Status.CodeReview => Color.Primary,
            Status.Deploy => Color.Primary,
            Status.Validation => Color.Primary,
            Status.Done => Color.Success,
            Status.Cancelled => Color.Success,
            Status.Canceled => Color.Success,
            _ => Color.Transparent
        };
    }

    public static string FormattedStartDate(this Issue issue)
    {
        return issue.StartDate().ToLocalDateOnly();
    }

    public static string FormattedDueDate(this Issue issue)
    {
        return issue.DueDate.ToLocalDateOnly();
    }

    public static DateTime? StartDate(this Issue issue)
    {
        var field = issue.CustomFields.GetCascadingSelectField("Start date");

        return DateTime.TryParse(field?.ParentOption, out var date)
            ? date
            : null;
    }

    public static void UpdateStartDate(this Issue issue, DateTime? startDate)
    {
        const string fieldId = "customfield_10421";

        var field = issue.CustomFields.SingleOrDefault(field =>
            string.Equals(field.Id, fieldId, StringComparison.InvariantCultureIgnoreCase));

        if (field is null)
        {
            issue.CustomFields.AddById(fieldId, startDate?.ToIsoDateString() ?? string.Empty);
            return;
        }

        field.Values = new[] { startDate.ToIsoDateString() };
    }


    public static List<BreadcrumbItem> ToBreadcrumbs(this Issue issue)
    {
        var items = new List<BreadcrumbItem>();

        if (!string.IsNullOrEmpty(issue.ParentIssueKey))
            items.Add(new(issue.ParentIssueKey, $"{issue.Jira.Url}browse/{issue.ParentIssueKey}"));
        items.Add(new(issue.Key.Value, $"{issue.Jira.Url}browse/{issue.Key}"));

        return items;
    }

    public static void AddComponents(this Issue issue, IEnumerable<ProjectComponent> components)
    {
        foreach (var component in components) issue.Components.Add(component);
    }
}