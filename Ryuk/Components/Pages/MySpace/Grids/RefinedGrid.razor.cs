namespace Ryuk.Components.Pages.MySpace.Grids;

using System.Collections.Frozen;
using Atlassian.Jira;
using Extensions;
using Microsoft.AspNetCore.Components;
using Model;

public partial class RefinedGrid
{
    HashSet<RefinedModel> _items = [];

    Jira jira = null!;
    [Inject] IServiceProvider Provider { get; init; } = null!;

    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }

    protected override void OnInitialized() => jira = Provider.GetRequiredKeyedService<Jira>("Company1");

    protected override async Task OnParametersSetAsync()
    {
        foreach (var issue in (Issues ?? FrozenSet<Issue>.Empty).Where(issue => issue.InStatus(Status.Refined))
                 .OrderBy(issue => issue.Status.Name))
        {
            var issueChangeLogItems = await issue.GetChangeLogsItems();
            _items.Add(new(issue, issueChangeLogItems));
        }
    }

    async Task CommittedItemChanges(RefinedModel item)
    {
        var issue = Issues?.GetByKey(item.Key);

        var errors = await jira.UpdateOriginalEstimateAsync(item.Key, item.OriginalEstimate).ToListAsync();
        if (errors.Count != 0) await DialogService.ShowMessageBox("Error", string.Join("; ", errors));

        try
        {
            if (issue != null)
            {
                issue.DueDate = item.DueDate;
                issue.UpdateStartDate(item.StartDate);

                await issue.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Error", e.Message);
        }
    }
}