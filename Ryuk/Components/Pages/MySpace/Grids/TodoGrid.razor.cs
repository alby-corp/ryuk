﻿namespace Ryuk.Components.Pages.MySpace.Grids;

using System.Collections.Frozen;
using Atlassian.Jira;
using Extensions;
using Microsoft.AspNetCore.Components;
using Model;

public partial class TodoGrid
{
    FrozenSet<ToDoModel>? _items;

    Jira jira = null!;
    [Inject] IServiceProvider Provider { get; init; } = null!;

    [Parameter] public required FrozenSet<Issue>? Issues { get; set; }

    protected override void OnInitialized() => jira = Provider.GetRequiredKeyedService<Jira>("Company1");

    protected override void OnParametersSet()
    {
        _items = Issues?
            .Where(issue => issue.InStatus(Status.Todo))
            .Select(issue => new ToDoModel(issue))
            .OrderByDescending(model => model.Status)
            .ThenBy(model => model.Key)
            .ToFrozenSet() ?? FrozenSet<ToDoModel>.Empty;
    }

    async Task CommittedItemChanges(ToDoModel item)
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