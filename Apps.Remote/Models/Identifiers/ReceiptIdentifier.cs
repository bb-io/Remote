using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class ReceiptIdentifier : ExpenseIdentifier
{
    [Display("Receipt ID"), DataSource(typeof(ReceiptDataSource))]
    public string ReceiptId { get; set; } = string.Empty;
}