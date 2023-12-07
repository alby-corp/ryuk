using Atlassian.Jira;
using Ryuk.Extensions;

namespace Ryuk.Components.Pages.MySpace.Forms;

public class CreateModel
{
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ticket { get; set; } = string.Empty;
    public JiraUser? Assignee { get; set; }
    public IssuePriority Priority { get; set; } = null!;
    public IssueType Type { get; set; } = null!;
    public IEnumerable<ProjectComponent> Components { get; set; } = [];
    public string Labels { get; set; } = string.Empty;
    public string OriginalEstimate { get; set; } = string.Empty;

    public static IEnumerable<string> ValidateMinCharacters(string summary, int minCharacters)
    {
        if (!string.IsNullOrEmpty(summary) && summary.Length < minCharacters)
            yield return $"Min {minCharacters} characters";
    }

    public static IEnumerable<string> ValidateTicketId(string ticketId)
    {
        if (!string.IsNullOrEmpty(ticketId) && !Regex.IssueKey().IsMatch(ticketId))
            yield return "Incorrect TicketID format";
    }
}