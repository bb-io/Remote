using Apps.Remote.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Remote.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    private static IEnumerable<ConnectionProperty> ConnectionProperties => new[]
    {
        new ConnectionProperty(CredsNames.BaseUrl)
        {
            DisplayName = "Base URL",
            Description = "For production use: https://gateway.remote.com, for sandbox use: https://gateway.remote-sandbox.com",
            Sensitive = false
        },
        new ConnectionProperty(CredsNames.ApiToken)
        {
            DisplayName = "API token",
            Description = "Bearer token for the API",
            Sensitive = true
        }
    };

    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = "Developer API key",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionUsage = ConnectionUsage.Actions,
            ConnectionProperties = ConnectionProperties
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values) =>
        values.Select(x =>
                new AuthenticationCredentialsProvider(AuthenticationCredentialsRequestLocation.None, x.Key, x.Value))
            .ToList();
}