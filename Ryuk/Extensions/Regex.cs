using System.Text.RegularExpressions;

namespace Ryuk.Extensions;

public partial class Regex
{
    [GeneratedRegex(@"^[A-Za-z]{3}-\d{4}$")]
    public static partial System.Text.RegularExpressions.Regex IssueKey();
}