using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Ryuk.Components.Pages.MySpace.Grids.Abstract;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class RefinedGrid : IDashboardGrid
{
    [Inject] IServiceProvider Provider { get; init; } = null!;
    [Inject] IDialogService  DialogService { get; set; } = null!;
    
    MudDataGrid<RefinedModel> grid;
    Jira jira = null!;
    
    public Task ReloadAsync() => grid!.ReloadServerData();
}