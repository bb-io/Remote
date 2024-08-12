using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class InvoiceScheduleIdentifier
{
    [Display("Invoice schedule ID"), DataSource(typeof(InvoiceScheduleDataSource))]
    public string InvoiceScheduleId { get; set; } = string.Empty;
}