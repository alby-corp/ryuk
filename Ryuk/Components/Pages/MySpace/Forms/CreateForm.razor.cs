using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Ryuk.Components.Pages.MySpace.Forms;

public partial class CreateForm
{
    readonly CreateModel _model = new();

    Jira _jira;

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }
    [Inject] IServiceProvider Provider { get; init; } = null!;

    void Close() => MudDialog.Close(DialogResult.Ok(true));

    protected override async Task OnInitializedAsync()
    {
        _jira = Provider.GetRequiredKeyedService<Jira>("Company1");

        var project = await _jira.Projects.GetProjectAsync("EIMMS");
        var components = await project.GetComponentsAsync();
        var types = await project.GetIssueTypesAsync();
        var priorities = await _jira.Priorities.GetPrioritiesAsync();
    }

    async Task CreateAsync()
    {
        var issue = _jira.CreateIssue("EIMMS");

        issue.Summary = _model.Summary;
        issue.Description = _model.Description;
        issue.Reporter = _model.Reporter;
        issue.Assignee = _model.Assignee;

        await issue.SaveChangesAsync();
    }
}