namespace Ryuk.Components.Pages.MySpace.Forms;

using Atlassian.Jira;
using System.Text.RegularExpressions;

public class CreateModel
{
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ticket { get; set; } = string.Empty;
    public JiraUser? Assignee { get; set; }
    public IssuePriority Priority { get; set; } = null!;
    public IssueType Type { get; set; } = null!;
    public IEnumerable<ProjectComponent> Components { get; set; } = null!;
    public string Labels { get; set; } = string.Empty;
    public string OriginalEstimate { get; set; } = string.Empty;

    public IEnumerable<string> ValidateMinCharacters(string ch, int minCharacters)
    {
        if (!string.IsNullOrEmpty(ch) && ch?.Length < minCharacters)
            yield return $"Min {minCharacters} characters";
    }

    public IEnumerable<string> ValidateTicketId(string ch)
    {
        Regex regex = new Regex(@"^[A-Za-z]{3}-\d{4}$");

        if (!string.IsNullOrEmpty(ch) && !regex.IsMatch(ch))
            yield return "Incorrect TicketID format";
    }
}