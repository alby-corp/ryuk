using System.Collections.Frozen;
using Atlassian.Jira;

namespace Ryuk.Services;

public class CacheService
{
    readonly HashSet<string> _projects = [];
    
    readonly List<ProjectComponent> _components = [];
    readonly List<CacheType> _types = [];
    readonly List<CacheStatus> _statues = [];

    public FrozenSet<ProjectComponent> SelectedComponents { get; private set; } = FrozenSet<ProjectComponent>.Empty;
    public FrozenSet<IssueType> SelectedTypes { get; private set; } = FrozenSet<IssueType>.Empty;
    public FrozenSet<IssueStatus> SelectedStatues { get; private set; } = FrozenSet<IssueStatus>.Empty;

    public async Task AddAsync(IEnumerable<Project> selectedProjects)
    {
        var projects = selectedProjects.ToFrozenSet();
        var keys = projects.Select(project => project.Key).ToFrozenSet();
        
        foreach (var project in projects)
        {
            var key = project.Key;
            if(!_projects.Contains(key)) await AddAsync(project, key);
        }
        
        SelectedComponents = _components.Where(component => keys.Contains(component.ProjectKey)).ToFrozenSet();
        SelectedTypes = _types.Where(type => keys.Contains(type.Key)).Select(type => type.Type).ToFrozenSet();
        SelectedStatues = _statues.Where(status => keys.Contains(status.Key)).Select(status => status.Status).ToFrozenSet();
    }
    
    async Task AddAsync(Project project, string key)
    {
        var components = (await project.GetComponentsAsync())
            .OrderBy(component => component.Name);

        var types = (await project.GetIssueTypesAsync())
            .OrderBy(type => type.Name)
            .Select(type => new CacheType(key, type))
            .ToHashSet();
        
        var statues = types
            .SelectMany(type => type.Type.Statuses)
            .OrderBy(status => status.Name)
            .Select(status => new CacheStatus(key, status))
            .Distinct();

        _projects.Add(key);
        
        _components.AddRange(components);
        _types.AddRange(types);
        _statues.AddRange(statues);
    }
}
    
public record CacheType(string Key, IssueType Type);
public record CacheStatus(string Key, IssueStatus Status);