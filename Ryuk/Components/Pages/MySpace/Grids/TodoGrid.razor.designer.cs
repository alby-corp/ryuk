using MudBlazor;
using Ryuk.Components.Pages.MySpace.Grids.Abstract;

namespace Ryuk.Components.Pages.MySpace.Grids;

using Microsoft.AspNetCore.Components;

public partial class TodoGrid : IDashboardGrid
{
    [Inject] IDialogService  DialogService { get; set; } = null!;

    MudDataGrid<ToDoModel> grid;
    
    public Task ReloadAsync() => grid!.ReloadServerData();
}