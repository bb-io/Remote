using Apps.Remote.DataSourceHandlers.Static;
using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Remote.Models.Requests.InvoiceSchedules;

public class ImportInvoiceRequest : EmploymentIdentifier
{
    public FileReference File { get; set; } = new();

    public string? Description { get; set; }

    [Display("Periodicity", Description = "Defines how often contractor invoices will be generate. By default we set it to 'monthly'"), StaticDataSource(typeof(PeriodicityDataSource))]
    public string? Periodicity { get; set; }
    
    [Display("Number of occurrences", Description = "Count of invoices that should be generated during schedule lifetime")]
    public int? NrOccurrences { get; set; }
}