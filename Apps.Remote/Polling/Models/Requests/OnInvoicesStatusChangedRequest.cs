using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.Polling.Models.Requests;

public class OnInvoicesStatusChangedRequest
{
    [StaticDataSource(typeof(InvoiceStatusDataSource))]
    public string Status { get; set; }
}