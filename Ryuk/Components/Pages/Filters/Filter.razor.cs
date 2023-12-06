using System.Collections.Frozen;
using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using Ryuk.Services;

namespace Ryuk.Components.Pages.Filters;

public partial class Filter
{
    [Inject] IServiceProvider Provider { get; init; } = null!;
    [Inject] CacheService Cache { get; init; } = null!;
    
    List<Project> Projects { get; set; } = [];
    
    FrozenSet<Project> SelectedProjects { get; set; } = FrozenSet<Project>.Empty;
    
    FrozenSet<ProjectComponent> SelectedIncludedComponents { get; set; } = FrozenSet<ProjectComponent>.Empty;
    FrozenSet<ProjectComponent> SelectedExcludedComponents { get; set; } = FrozenSet<ProjectComponent>.Empty;
   
    FrozenSet<IssueType> SelectedIncludedTypes { get; set; } = FrozenSet<IssueType>.Empty;
    FrozenSet<IssueType> SelectedExcludedTypes { get; set; } = FrozenSet<IssueType>.Empty;
    
    FrozenSet<IssueStatus> SelectedIncludedStatues { get; set; } = FrozenSet<IssueStatus>.Empty;
    FrozenSet<IssueStatus> SelectedExcludedStatues { get; set; } = FrozenSet<IssueStatus>.Empty;

    readonly FilterModel _model = new();
    
    protected override async Task OnInitializedAsync()
    {
        var jira = Provider.GetRequiredKeyedService<Jira>("Company1");
        
        Projects = (await jira.Projects.GetProjectsAsync())
            .OrderBy(project => project.Key)
            .ToList();
    }

    Task SelectedProjectChanged(IEnumerable<Project> selectedProjects)
    {
        var projects = selectedProjects.ToFrozenSet();
        
        SelectedProjects = projects;
        return Cache.AddAsync(projects);
    }

    void SelectedIncludedComponentsChanged(IEnumerable<ProjectComponent> selectedComponents) => SelectedIncludedComponents = selectedComponents.ToFrozenSet();

    void SelectedExcludedComponentsChanged(IEnumerable<ProjectComponent> selectedComponents) => SelectedExcludedComponents = selectedComponents.ToFrozenSet();

    void SelectedIncludedTypesChanged(IEnumerable<IssueType> selectedTypes) => SelectedIncludedTypes = selectedTypes.ToFrozenSet();

    void SelectedExcludedTypesChanged(IEnumerable<IssueType> selectedTypes) => SelectedExcludedTypes = selectedTypes.ToFrozenSet();

    void SelectedIncludedStatuesChanged(IEnumerable<IssueStatus> selectedStatues) => SelectedIncludedStatues = selectedStatues.ToFrozenSet();

    void SelectedExcludedStatuesChanged(IEnumerable<IssueStatus> selectedStatues) => SelectedExcludedStatues = selectedStatues.ToFrozenSet();
}