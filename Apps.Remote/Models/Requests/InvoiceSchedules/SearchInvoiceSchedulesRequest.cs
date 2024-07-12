using Blackbird.Applications.Sdk.Common;
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

    [Display("Status"), JsonProperty("status")]
    public string? Status { get; set; }

    [Display("Employment ID"), JsonProperty("employment_id")]
    public string? EmploymentId { get; set; }

    [Display("Periodicity"), JsonProperty("periodicity")]
    public string? Periodicity { get; set; }

    [Display("Currency"), JsonProperty("currency")]
    public string? Currency { get; set; }
    
    [Display("Number")]
    public string? Number { get; set; }
}