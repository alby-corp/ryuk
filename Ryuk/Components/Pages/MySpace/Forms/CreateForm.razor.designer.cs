namespace Ryuk.Components.Pages.MySpace.Forms;

using Microsoft.AspNetCore.Components;
using MudBlazor;

public partial class CreateForm
{
    [Inject] IDialogService DialogService { get; set; } = null!;

    const string Key = "Company1";
    const uint MinCharacters = 4;
    bool success;
    string[] errors = { };
    MudForm form = null!;
}