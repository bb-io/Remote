using System.Globalization;
using Apps.Remote.Actions;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Requests.Invoices;
using Apps.Remote.Models.Responses.Invoices;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.DataSourceHandlers;

public class InvoiceDataSource(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var employmentActions = new InvoiceActions(InvocationContext);
        var employmentsResponse = await employmentActions.SearchInvoices(new SearchInvoicesRequest());

        return employmentsResponse.Invoices?
                   .Where(x => context.SearchString == null || BuildReadableName(x).Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Id, BuildReadableName)
               ?? new Dictionary<string, string>();
    }
    
    private string BuildReadableName(InvoiceResponse invoiceSchedule)
    {
        return $"({invoiceSchedule.Date.ToString("yyyy MMMM dd", CultureInfo.InvariantCulture)}) {invoiceSchedule.Currency}";
    }
}