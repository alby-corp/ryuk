using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Ryuk.Extensions;

namespace Ryuk.Components.Pages.MySpace.Forms;

public partial class CreateForm
{
    const string ProjectKey = "EIMMS";
    readonly CreateModel _model = new();
    IEnumerable<ProjectComponent> _components = [];

    Jira _jira = null!;

    IEnumerable<IssuePriority> _priorities = [];
    IEnumerable<IssueType> _types = [];
    List<string> _labels = new();

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }
    [Inject] IServiceProvider Provider { get; init; } = null!;

    void Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    protected override async Task OnInitializedAsync()
    {
        _jira = Provider.GetRequiredKeyedService<Jira>(Key);

        var project = await _jira.Projects.GetProjectAsync(ProjectKey);

        _components = (await project.GetComponentsAsync()).OrderBy(component => component.Name).ToList();
        _types = (await project.GetIssueTypesAsync()).OrderBy(type => type.Name).ToList();
        _priorities = (await _jira.Priorities.GetPrioritiesAsync()).OrderBy(priority => priority.Name).ToList();
    }

    async void Validate()
    {
        await form!.Validate();
        if (success) await CreateAsync();
    }

    async Task CreateAsync()
    {
        try
        {
            var issue = _jira.CreateIssue(ProjectKey);

            issue.Type = _model.Type;
            issue.Summary = string.IsNullOrEmpty(_model.Ticket)
                ? _model.Summary
                : string.Concat(_model.Ticket, " | ", _model.Summary);
            issue.Description = _model.Description;
            issue.Assignee = _model.Assignee is not null ? _model.Assignee.Username : string.Empty;
            issue.Priority = _model.Priority;
            issue.AddComponents(_model.Components);
            issue.AddLabels(_labels);

            var resultIssue = await issue.SaveChangesAsync();

            if (!string.IsNullOrEmpty(_model.OriginalEstimate) && resultIssue != null)
            {
                var list = await _jira.UpdateOriginalEstimateAsync($"{resultIssue.Key}", _model.OriginalEstimate)
                    .ToListAsync();
                if (list.Count != 0) await DialogService.ShowMessageBox("Error", string.Join("; ", list));
            }
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Error", e.Message);
        }
    }

    Task<IEnumerable<JiraUser>> GetAssignee(string value)
    {
        return _jira.Users.SearchAssignableUsersForProjectAsync(value, ProjectKey);
    }
    
    void SetValue()
    {
        if (!string.IsNullOrEmpty(_model.Labels))
            _labels.Add(_model.Labels);
    }

    void Closed(MudChip chip) => _labels.Remove(chip.Text);
}