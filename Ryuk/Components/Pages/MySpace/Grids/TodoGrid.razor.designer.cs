using MudBlazor;
using Ryuk.Components.Pages.MySpace.Grids.Abstract;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class TodoGrid : IDashboardGrid
{
    MudDataGrid<ToDoModel> grid;
    
    public Task ReloadAsync() => grid!.ReloadServerData();
}