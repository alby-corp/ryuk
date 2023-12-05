using System.Collections.Frozen;
using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using Ryuk.Extensions;
using Ryuk.Model;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class TodoGrid
{
    FrozenSet<ToDoModel>? _items;
    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }

    protected override void OnParametersSet() =>
        _items = Issues
            ?.Where(issue => issue.InStatus(Status.Todo))
            .Select(issue => new ToDoModel(issue))
            .OrderBy(model => model.Status)
            .ToFrozenSet() ?? FrozenSet<ToDoModel>.Empty;
}