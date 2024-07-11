using System.Globalization;
using Apps.Remote.Actions;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Requests.InvoiceSchedules;
using Apps.Remote.Models.Responses.InvoiceSchedules;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.DataSourceHandlers;

public class InvoiceScheduleDataSource(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var employmentActions = new InvoiceSchedulesActions(InvocationContext);
        var employmentsResponse = await employmentActions.SearchInvoiceSchedules(new SearchInvoiceSchedulesRequest());

        return employmentsResponse.InvoiceSchedules?
                   .Where(x => context.SearchString == null || BuildReadableName(x).Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Id, BuildReadableName)
               ?? new Dictionary<string, string>();
    }
    
    private string BuildReadableName(InvoiceScheduleResponse invoiceSchedule)
    {
        return $"({invoiceSchedule.StartDate.ToString("yyyy MMMM dd", CultureInfo.InvariantCulture)}) {invoiceSchedule.Currency}";
    }
}