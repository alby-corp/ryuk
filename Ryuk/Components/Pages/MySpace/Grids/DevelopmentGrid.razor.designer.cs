using MudBlazor;
using Ryuk.Components.Pages.MySpace.Grids.Abstract;

namespace Ryuk.Components.Pages.MySpace.Grids;

using Microsoft.AspNetCore.Components;

public partial class DevelopmentGrid : IDashboardGrid
{
    MudDataGrid<DevelopmentModel> grid;
    [Inject] IDialogService DialogService { get; set; } = null!;
    
    public Task ReloadAsync() => grid!.ReloadServerData();
}