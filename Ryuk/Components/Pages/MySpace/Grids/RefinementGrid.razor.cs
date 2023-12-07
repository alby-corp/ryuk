using System.Collections.Frozen;
using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using Ryuk.Extensions;
using Ryuk.Model;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class RefinementGrid
{
    [Inject] IServiceProvider Provider { get; init; } = null!;

    Jira jira = null!;

    FrozenSet<RefinementModel>? _items;
    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }
    protected override void OnInitialized() => jira = Provider.GetRequiredKeyedService<Jira>("Company1");
    protected override void OnParametersSet() =>
        _items = Issues?
            .Where(issue => issue.InStatus(Status.Backlog, Status.Refinement))
            .Select(issue => new RefinementModel(issue))
            .OrderByDescending(model => model.Status)
            .ThenBy(model => model.Key)
            .ToFrozenSet() ?? FrozenSet<RefinementModel>.Empty;
    
    async Task CommittedItemChanges(RefinementModel item)
    {
        var errors = await jira.UpdateOriginalEstimateAsync(item.Key, item.OriginalEstimate).ToListAsync();
        if (errors.Count != 0) await DialogService.ShowMessageBox("Error", string.Join("; ", errors));

    }
    
    
}