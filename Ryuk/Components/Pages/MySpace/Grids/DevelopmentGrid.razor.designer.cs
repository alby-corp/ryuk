using MudBlazor;
using Ryuk.Components.Pages.MySpace.Grids.Abstract;

namespace Ryuk.Components.Pages.MySpace.Grids;

public partial class DevelopmentGrid : IDashboardGrid
{
    MudDataGrid<DevelopmentModel> grid;
    
    public Task ReloadAsync() => throw new NotImplementedException();
}