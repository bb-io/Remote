using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Responses.CustomFields;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Remote.DataSourceHandlers;

public class CustomFieldDataSource(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new ApiRequest("/v1/custom-fields", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<CustomFieldsResponse>(request);
        return response.CustomFields?
                   .Where(x => context.SearchString == null || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Id, x => x.Name)
               ?? new Dictionary<string, string>();
    }
}