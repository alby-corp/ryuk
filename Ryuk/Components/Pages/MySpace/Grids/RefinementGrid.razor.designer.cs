using Atlassian.Jira;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Ryuk.Components.Pages.MySpace.Grids.Abstract;

namespace Ryuk.Components.Pages.MySpace.Grids;

using static Task;

public partial class RefinementGrid : IDashboardGrid
{
    private readonly IMask tagMask = new RegexMask(@"");
    [Inject] IDialogService  DialogService { get; set; } = null!;
    
    MudDataGrid<RefinementModel> grid;
    public Task ReloadAsync() => grid?.ReloadServerData() ?? CompletedTask;
}