namespace Ryuk.Model;

public class IssueChangeLogItem(DateTime creationDate, Atlassian.Jira.IssueChangeLogItem logItem)
{
    public DateTime CreatedDate { get; } = creationDate;
    public Atlassian.Jira.IssueChangeLogItem LogItem { get; } = logItem;
}