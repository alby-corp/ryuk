using MudBlazor.Services;

namespace Ryuk;

using Atlassian.Jira;
using Microsoft.Extensions.Options;
using Options;

public static class Bootstrapper
{
    public static void AddRyuk(this WebApplicationBuilder builder)
    {
        builder.AddClients();
        builder.Services.AddMudServices();
    }

    static void AddClients(this WebApplicationBuilder builder)
    {
        var children = builder.Configuration
            .GetSection("Ryuk:Companies")
            .GetChildren();

        foreach (var child in children)
        {
            builder.Services
                .AddOptions<CompanyOptions>(child.Key)
                .Bind(child)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddKeyedScoped<Jira>(child.Key, (provider, _) =>
            {
                var auth = provider.GetRequiredService<IOptionsSnapshot<CompanyOptions>>().Get(child.Key).Auth;
                return Jira.CreateRestClient(auth.Origin, auth.Username, auth.Password);
            });
        }
    }
}