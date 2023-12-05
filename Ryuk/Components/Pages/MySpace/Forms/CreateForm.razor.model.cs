namespace Ryuk.Components.Pages.MySpace.Forms;

public class CreateModel
{
    public string Summary { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Ticket { get; set; } = string.Empty;
    public string Reporter { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Components { get; set; } = string.Empty;
    public string Labels { get; set; } = string.Empty;
    public string OriginalEstimate { get; set; } = string.Empty;
}