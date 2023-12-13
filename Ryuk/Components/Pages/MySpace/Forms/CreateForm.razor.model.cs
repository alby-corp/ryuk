using System.ComponentModel.DataAnnotations;
using Atlassian.Jira;

namespace Ryuk.Components.Pages.MySpace.Forms;

public class CreateModel
{
    [Required]
    [MinLength(4, ErrorMessage = "Summary must be at least 4 characters")]
    public string Summary { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [RegularExpression(@"^[A-Za-z]{3}-\d{4}$", ErrorMessage = "Incorrect TicketID format")]
    public string Ticket { get; set; } = string.Empty;
    
    public JiraUser? Assignee { get; set; }
    
    [Required]
    public IssuePriority Priority { get; set; } = null!;
    
    [Required]
    public IssueType Type { get; set; } = null!;
    
    public IEnumerable<ProjectComponent> Components { get; set; } = [];
    
    public string Labels { get; set; } = string.Empty;
    
    [RegularExpression(@"^ *([0-9]+[WwDdHhMm])( +[0-9]+[WwDdHhMm])* *$", ErrorMessage = "Incorrect OriginalEstimate format")]
    public string OriginalEstimate { get; set; } = string.Empty;
}