using Apps.Remote.DataSourceHandlers.Static;
using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.Models.Requests.InvoiceSchedules;

public class UpdateInvoiceScheduleRequest : InvoiceScheduleIdentifier
{
    public string? Currency { get; set; } = string.Empty;
    
    [Display("Start date")]
    public DateTime? StartDate { get; set; }
    
    [StaticDataSource(typeof(PeriodicityDataSource))]
    public string? Periodicity { get; set; } = string.Empty;

    public List<double>? Amounts { get; set; } = new();
    
    public List<string>? Descriptions { get; set; } = new();
    
    public string? Note { get; set; }
    
    [Display("Number of occurrences")]
    public int? NrOccurrences { get; set; }
    
    [Display("Invoice number")]
    public string? Number { get; set; }
}