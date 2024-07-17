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
    public List<InvoiceItemResponse> Items { get; set; } = new();

    [Display("Item amounts")]
    public List<double> ItemAmounts { get; set; } = new();
    
    [Display("Item descriptions")] 
    public List<string> ItemDescriptions { get; set; } = new();

    [JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [JsonProperty("note")]
    public string Note { get; set; } = string.Empty;

    [Display("Start Date"), JsonProperty("start_date")]
    public DateTime? StartDate { get; set; }

    [Display("Total Amount"), JsonProperty("total_amount")]
    public double? TotalAmount { get; set; }

    [Display("Employment ID"), JsonProperty("employment_id")]
    public string EmploymentId { get; set; } = string.Empty;

    [Display("Next Invoice At"), JsonProperty("next_invoice_at")]
    public DateTime? NextInvoiceAt { get; set; }

    [Display("Number of Occurrences"), JsonProperty("nr_occurrences")]
    public int? NumberOfOccurrences { get; set; }

    [JsonProperty("periodicity")]
    public string Periodicity { get; set; } = string.Empty;
    
    public void SetItemAmountsAndDescriptions()
    {
        ItemAmounts = Items.Select(x => (double)x.Amount).ToList();
        ItemDescriptions = Items.Select(x => x.Description).ToList();
    }
}