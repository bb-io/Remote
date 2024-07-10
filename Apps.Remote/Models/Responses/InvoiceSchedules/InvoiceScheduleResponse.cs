using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.InvoiceSchedules;

public class InvoiceScheduleResponse
{
    [Display("Invoice Schedule ID"), JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [Display("Invoice Number"), JsonProperty("number")]
    public string Number { get; set; } = string.Empty;

    [JsonProperty("items")]
    public List<InvoiceItem> Items { get; set; } = new();

    [JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonProperty("note")]
    public string Note { get; set; } = string.Empty;

    [Display("Start Date"), JsonProperty("start_date")]
    public DateTime StartDate { get; set; }

    [Display("Total Amount"), JsonProperty("total_amount")]
    public decimal TotalAmount { get; set; }

    [Display("Employment ID"), JsonProperty("employment_id")]
    public string EmploymentId { get; set; } = string.Empty;

    [Display("Next Invoice At"), JsonProperty("next_invoice_at")]
    public DateTime? NextInvoiceAt { get; set; }

    [Display("Number of Occurrences"), JsonProperty("nr_occurrences")]
    public int NumberOfOccurrences { get; set; }

    [JsonProperty("periodicity")]
    public string Periodicity { get; set; } = string.Empty;
}

public class InvoiceItem
{
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("amount")]
    public decimal Amount { get; set; }
}