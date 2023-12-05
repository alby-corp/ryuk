using System.Collections.Frozen;
using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Ryuk.Components.Pages.MySpace.Forms;
using Ryuk.Model;
using Ryuk.Options;

namespace Ryuk.Components.Pages.MySpace;

public partial class Dashboard
{
    FrozenSet<Issue>? _issues;
    [Inject] IServiceProvider Provider { get; init; } = null!;
    [Inject] IDialogService DialogService { get; init; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var username = Provider.GetRequiredService<IOptionsSnapshot<CompanyOptions>>().Get(Key).Auth.Username;
        var jql = $@"project = EIMMS AND assignee = ""{username}"" AND Status not in (""{Status.Cancelled}"", ""{Status.Canceled}"") AND created > endOfMonth(-{MonthsAgo})";

        var jira = Provider.GetRequiredKeyedService<Jira>(Key);
        var result = await jira.Issues.GetIssuesFromJqlAsync(jql, int.MaxValue);
        
        _issues = result.ToFrozenSet();
    }

    void Create()
    {
        var options = new DialogOptions { CloseOnEscapeKey = false, CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
        DialogService.Show<CreateForm>(string.Empty, options);
    }

    public void NotifyChanged() => StateHasChanged();
}