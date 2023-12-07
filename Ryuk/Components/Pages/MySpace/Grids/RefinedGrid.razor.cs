using System.Collections.Frozen;
using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using Ryuk.Extensions;
using Ryuk.Model;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class RefinedGrid
{
    FrozenSet<RefinedModel>? _items;
    [Inject] IServiceProvider Provider { get; init; } = null!;
    
    Jira jira = null!;

    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }

    protected override void OnInitialized() => jira = Provider.GetRequiredKeyedService<Jira>("Company1");

    protected override void OnParametersSet() =>
        _items = Issues?
            .Where(issue => issue.InStatus(Status.Refined))
            .Select(issue => new RefinedModel(issue))
            .OrderBy(model => model.Status)
            .ToFrozenSet() ?? FrozenSet<RefinedModel>.Empty;

    async Task CommittedItemChanges(RefinedModel item)
    {
        var issue = Issues.GetByKey(item.Key);

        var errors = await jira.UpdateOriginalEstimateAsync(item.Key, item.OriginalEstimate).ToListAsync();
        if (errors.Count != 0) await DialogService.ShowMessageBox("Error", string.Join("; ", errors));

        try
        {
            issue.DueDate = item.DueDate;
            issue.UpdateStartDate(item.StartDate);

            await issue.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await DialogService.ShowMessageBox("Error", e.Message);
        }
    }
}