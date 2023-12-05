using System.Collections.Frozen;
using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Ryuk.Extensions;
using Ryuk.Model;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class DevelopmentGrid
{
    FrozenSet<DevelopmentModel>? _items;
    Jira _jira = null!;

    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }

    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] IServiceProvider Provider { get; init; } = null!;

    protected override void OnInitialized() => _jira = Provider.GetRequiredKeyedService<Jira>("Company1");

    protected override void OnParametersSet()
    {
        _items = Issues?
            .Where(issue => issue.NotInStatus(Status.Backlog, Status.Refinement, Status.Refined, Status.Todo, Status.Canceled, Status.Cancelled, Status.Done))
            .Select(issue => new DevelopmentModel(issue))
            .ToFrozenSet() ?? FrozenSet<DevelopmentModel>.Empty;
    }

    async Task CommittedItemChanges(DevelopmentModel item)
    {
        var errors = await _jira.UpdateRemainingEstimateAsync(item.Key, item.RemainingEstimate).ToListAsync();
        if (errors.Any()) await DialogService.ShowMessageBox("Error", string.Join("; ", errors));
    }
}