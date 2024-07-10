using Apps.Remote.DataSourceHandlers.Static;
using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.Models.Requests.InvoiceSchedules;

public class CreateInvoiceScheduleRequest : EmploymentIdentifier
{
    [Display("Currency", Description = "Currency code in ISO 4217 format"), StaticDataSource(typeof(CurrencyDataSource))]
    public string Currency { get; set; } = string.Empty;
    
    [Display("Start date")]
    public DateTime StartDate { get; set; }
    
    [StaticDataSource(typeof(PeriodicityDataSource))]
    public string Periodicity { get; set; } = string.Empty;

    public List<double> Amounts { get; set; } = new();
    
    public List<string> Descriptions { get; set; } = new();
    
    [Display("Note", Description = "Custom defined note")]
    public string? Note { get; set; }
    
    [Display("Number of occurrences", Description = "Count of invoices that should be generated during schedule lifetime")]
    public int? NrOccurrences { get; set; }
    
    [Display("Number", Description = "Invoice identifier")]
    public string? Number { get; set; }
}