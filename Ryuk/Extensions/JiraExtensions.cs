namespace Ryuk.Extensions;

using Atlassian.Jira;
using RestSharp;

public static class JiraExtensions
{
    public static async IAsyncEnumerable<KeyValuePair<string, string>> UpdateOriginalEstimateAsync(this Jira jira, string key, string value)
    {
        var request = new RestRequest($"rest/api/2/issue/{key}")
        {
            Method = Method.PUT,
            RequestFormat = DataFormat.Json
        };

        var body = new { update = new { timetracking = new[] { new { edit = new { originalEstimate = value } } } } };
        request.AddJsonBody(body);

        var errors = new Dictionary<string, string>();

        try
        {
            await jira.RestClient.ExecuteRequestAsync(request);
        }
        catch (Exception e)
        {
            errors = e.ExtractErrors();
        }

        foreach (var error in errors) yield return error;
    }

    public static async IAsyncEnumerable<KeyValuePair<string, string>> UpdateRemainingEstimateAsync(this Jira jira, string key, string value)
    {
        var request = new RestRequest($"rest/api/2/issue/{key}")
        {
            Method = Method.PUT,
            RequestFormat = DataFormat.Json
        };

        var body = new { update = new { timetracking = new[] { new { edit = new { remainingEstimate = value } } } } };
        request.AddJsonBody(body);

        var errors = new Dictionary<string, string>();

        try
        {
            await jira.RestClient.ExecuteRequestAsync(request);
        }
        catch (Exception e)
        {
            errors = e.ExtractErrors();
        }

        foreach (var error in errors) yield return error;
    }
}