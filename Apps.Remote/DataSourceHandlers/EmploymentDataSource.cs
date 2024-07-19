using Apps.Remote.Actions;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Requests.Employments;
using Apps.Remote.Models.Responses.Employments;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.DataSourceHandlers;

public class EmploymentDataSource(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var employmentActions = new EmploymentActions(InvocationContext);
        var employmentsResponse = await employmentActions.SearchEmployments(new SearchEmploymentsRequest());

        return employmentsResponse.Employments?
                   .Where(x => context.SearchString == null || x.FullName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Id, x => x.FullName)
               ?? new Dictionary<string, string>();
    }
}