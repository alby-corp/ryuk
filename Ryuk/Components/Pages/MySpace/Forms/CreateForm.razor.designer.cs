namespace Ryuk.Components.Pages.MySpace.Forms;

using Microsoft.AspNetCore.Components;
using MudBlazor;

public partial class CreateForm
{
    [Inject] IDialogService DialogService { get; set; } = null!;

    const string Key = "Company1";
    
    bool success;
    string[] errors = [];
}