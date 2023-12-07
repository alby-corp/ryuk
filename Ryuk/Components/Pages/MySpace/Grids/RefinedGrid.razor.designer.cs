using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Ryuk.Components.Pages.MySpace.Grids.Abstract;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class RefinedGrid : IDashboardGrid
{
    [Inject] IDialogService  DialogService { get; set; } = null!;
    
    MudDataGrid<RefinedModel> grid;
    
    public Task ReloadAsync() => grid!.ReloadServerData();
}