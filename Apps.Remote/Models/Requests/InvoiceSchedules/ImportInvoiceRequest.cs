using Apps.Remote.DataSourceHandlers.Static;
using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Remote.Models.Requests.InvoiceSchedules;

public class ImportInvoiceRequest : ContractorIdentifier
{
    public FileReference File { get; set; } = new();

    public string? Description { get; set; }

    [Display("Periodicity", Description = "Defines how often contractor invoices will be generated. By default we set it to 'monthly'"), StaticDataSource(typeof(PeriodicityDataSource))]
    public string? Periodicity { get; set; }
    
    [Display("Number of occurrences", Description = "Count of invoices that should be generated during schedule lifetime")]
    public int? NrOccurrences { get; set; }
    
    [Display("Number", Description = "Invoice identifier")]
    public string? Number { get; set; }

    [Display("Start date", Description = "Date of the first contractor invoice generation. If you will not specify it, it will be set to the current date + 7 days")]
    public DateTime? StartDate { get; set; }
}