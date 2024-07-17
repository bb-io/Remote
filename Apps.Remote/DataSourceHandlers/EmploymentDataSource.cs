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
                   .Where(x => context.SearchString == null || BuildReadableName(x).Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Id, BuildReadableName)
               ?? new Dictionary<string, string>();
    }
    
    private string BuildReadableName(EmploymentResponse employment)
    {
        return $"[{employment.Type}] {employment.FullName}";
    }
}