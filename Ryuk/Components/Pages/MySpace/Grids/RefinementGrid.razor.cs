using System.Collections.Frozen;
using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using Ryuk.Extensions;
using Ryuk.Model;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class RefinementGrid
{
    FrozenSet<RefinementModel>? _items;
    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }

    protected override void OnParametersSet() =>
        _items = Issues?
            .Where(issue => issue.InStatus(Status.Backlog, Status.Refinement))
            .Select(issue => new RefinementModel(issue))
            .OrderByDescending(model => model.Status)
            .ThenBy(model => model.Key)
            .ToFrozenSet() ?? FrozenSet<RefinementModel>.Empty;
}