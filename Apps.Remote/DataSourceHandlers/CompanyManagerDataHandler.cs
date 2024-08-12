using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Requests.Users;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Remote.DataSourceHandlers;

public class CompanyManagerDataHandler(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new ApiRequest("/v1/company-managers", Method.Get, Creds);
        var items = await Client.Paginate<UserResponse, CompanyManagersPaginationResponse>(request);

        return items
            .Where(x => context.SearchString == null ||
                        x.UserName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(40)
            .ToDictionary(x => x.UserId, x => x.UserName);
    }
}