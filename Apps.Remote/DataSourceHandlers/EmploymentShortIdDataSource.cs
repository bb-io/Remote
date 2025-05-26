using Apps.Remote.Actions;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Requests.Employments;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Remote.DataSourceHandlers
{
    public class EmploymentShortIdDataSource(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
    {
        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
            CancellationToken cancellationToken)
        {
            var employmentActions = new EmploymentActions(InvocationContext);
            var employmentsResponse = await employmentActions.SearchEmployments(new SearchEmploymentsRequest());

            return employmentsResponse.Employments?
                       .Where(x => context.SearchString == null || x.FullName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                       .ToDictionary(x => x.ShortId, x => x.FullName)
                   ?? new Dictionary<string, string>();
        }
    }
}