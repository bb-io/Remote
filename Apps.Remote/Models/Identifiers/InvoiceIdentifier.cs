using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class InvoiceIdentifier
{
    [Display("Invoice ID"), DataSource(typeof(InvoiceDataSource))]
    public string InvoiceId { get; set; } = string.Empty;
}