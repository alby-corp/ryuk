namespace Ryuk.Components.Pages.MySpace.Forms;

using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Extensions;

public partial class CreateForm
{
    readonly CreateModel model = new();

    Jira jira = null!;

    [CascadingParameter] public required MudDialogInstance MudDialog { get; set; }
    [Inject] IServiceProvider Provider { get; init; } = null!;

    IEnumerable<IssuePriority> priorities = [];
    IEnumerable<ProjectComponent> components = [];
    IEnumerable<IssueType> types = [];
    const string projectKey = "EIMMS";

    void Close() => MudDialog.Close(DialogResult.Ok(true));

    protected override async Task OnInitializedAsync()
    {
        jira = Provider.GetRequiredKeyedService<Jira>(Key);

        var project = await jira.Projects.GetProjectAsync(projectKey);
        components = (await project.GetComponentsAsync()).OrderBy(component => component.Name).ToList();
        types = (await project.GetIssueTypesAsync()).OrderBy(type => type.Name).ToList();
        priorities = (await jira.Priorities.GetPrioritiesAsync()).OrderBy(priority => priority.Name).ToList();
    }
    
    async void Validate()
    {
        await form.Validate();
        if (success)
        {
            await CreateAsync();
        }
    }

    async Task CreateAsync()
    {
        try
        {
            var issue = jira.CreateIssue(projectKey);

            issue.Type = model.Type;
            issue.Summary = string.IsNullOrEmpty(model.Ticket) ? model.Summary : string.Concat(model.Ticket, "|", model.Summary);
            issue.Description = model.Description;
            issue.Assignee = (model.Assignee != null) ? model.Assignee.Username : "";
            issue.Priority = model.Priority;
            model.Components.ToList().ForEach(issue.Components.Add);
            issue.Labels.Add(model.Labels);

            var resultIssue = await issue.SaveChangesAsync();
            
            if(!string.IsNullOrEmpty(model.OriginalEstimate) && resultIssue != null)
            {
                var errors = await jira.UpdateOriginalEstimateAsync(resultIssue.Key.ToString(), model.OriginalEstimate).ToListAsync();
                if (errors.Any()) await DialogService.ShowMessageBox("Error", string.Join("; ", errors));
            }
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Error", e.Message);
        }
    }

    async Task<IEnumerable<JiraUser>> GetAssignee(string value)
    {
        return await jira.Users.SearchAssignableUsersForProjectAsync(value, projectKey, 0, 50);
    }
}