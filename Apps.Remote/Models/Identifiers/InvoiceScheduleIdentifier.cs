using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Identifiers;

public class InvoiceScheduleIdentifier
{
    [Display("Invoice Schedule ID")]
    public string InvoiceScheduleId { get; set; } = string.Empty;
}