namespace Ryuk.Components.Pages.MySpace.Grids;

using System.Collections.Frozen;
using Atlassian.Jira;
using Extensions;
using Microsoft.AspNetCore.Components;
using Model;

public partial class DevelopmentGrid
{
    FrozenSet<DevelopmentModel>? _items;
    Jira _jira = null!;

    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }

    [Inject] IServiceProvider Provider { get; init; } = null!;

    protected override void OnInitialized() => _jira = Provider.GetRequiredKeyedService<Jira>("Company1");

    protected override void OnParametersSet()
    {
        _items = Issues?
            .Where(issue => issue.NotInStatus(Status.Backlog, Status.Refinement, Status.Refined, Status.Todo,
                Status.Canceled, Status.Cancelled, Status.Done))
            .Select(issue => new DevelopmentModel(issue))
            .ToFrozenSet() ?? FrozenSet<DevelopmentModel>.Empty;
    }

    async Task CommittedItemChanges(DevelopmentModel item)
    {
        var errors = await _jira.UpdateRemainingEstimateAsync(item.Key, item.RemainingEstimate).ToListAsync();
        if (errors.Count != 0) await DialogService.ShowMessageBox("Error", string.Join("; ", errors));
    }
}