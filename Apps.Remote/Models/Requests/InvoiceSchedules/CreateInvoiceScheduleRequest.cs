using Apps.Remote.DataSourceHandlers.Static;
using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Remote.Models.Requests.InvoiceSchedules;

public class CreateInvoiceScheduleRequest : ContractorIdentifier
{
    [Display("Currency", Description = "Currency code in ISO 4217 format"), StaticDataSource(typeof(CurrencyDataSource))]
    public string Currency { get; set; } = string.Empty;
    
    [Display("Start date", Description = "Date of the first contractor invoice generation")]
    public DateTime StartDate { get; set; }
    
    [Display("Periodicity", Description = "Defines how often contractor invoices will be generated"), StaticDataSource(typeof(PeriodicityDataSource))]
    public string Periodicity { get; set; } = string.Empty;

    [Display("Amounts", Description = "List of amounts that will be used to generate invoices")]
    public List<double> Amounts { get; set; } = new();
    
    [Display("Descriptions", Description = "List of descriptions that will be used to generate invoices")]
    public List<string> Descriptions { get; set; } = new();
    
    [Display("Note", Description = "Custom defined note")]
    public string? Note { get; set; }
    
    [Display("Number of occurrences", Description = "Count of invoices that should be generated during schedule lifetime")]
    public int? NrOccurrences { get; set; }
    
    [Display("Invoice number", Description = "Invoice identifier")]
    public string? Number { get; set; }
}