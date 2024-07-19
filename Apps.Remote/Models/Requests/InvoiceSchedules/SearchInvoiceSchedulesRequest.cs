using Apps.Remote.DataSourceHandlers;
using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Requests.InvoiceSchedules;

public class SearchInvoiceSchedulesRequest
{
    [Display("Start date from"), JsonProperty("start_date_from")]
    public DateTime? StartDateFrom { get; set; }

    [Display("Start date to"), JsonProperty("start_date_to")]
    public DateTime? StartDateTo { get; set; }

    [Display("Next invoice date from"), JsonProperty("next_invoice_date_from")]
    public DateTime? NextInvoiceDateFrom { get; set; }

    [Display("Next invoice date to"), JsonProperty("next_invoice_date_to")]
    public DateTime? NextInvoiceDateTo { get; set; }

    [Display("Status"), JsonProperty("status"), StaticDataSource(typeof(InvoiceSchedulesDataSource))]
    public string? Status { get; set; }

    [Display("Employment ID"), JsonProperty("employment_id"), DataSource(typeof(ContractorDataSource))]
    public string? EmploymentId { get; set; }

    [Display("Periodicity"), JsonProperty("periodicity"), StaticDataSource(typeof(PeriodicityDataSource))]
    public string? Periodicity { get; set; }

    [Display("Currency"), JsonProperty("currency"), StaticDataSource(typeof(CurrencyDataSource))]
    public string? Currency { get; set; }
    
    [Display("Invoice number")]
    public string? Number { get; set; }
}