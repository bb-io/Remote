using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.Invoices;

public class InvoiceResponse
{
    [Display("Amount"), JsonProperty("amount")]
    public double Amount { get; set; }

    [Display("Approved At"), JsonProperty("approved_at")]
    public DateTime? ApprovedAt { get; set; }

    [Display("Contractor Invoice Schedule ID"), JsonProperty("contractor_invoice_schedule_id")]
    public string ContractorInvoiceScheduleId { get; set; } = string.Empty;

    [Display("Currency"), JsonProperty("currency")]
    public string Currency { get; set; } = string.Empty;

    [Display("Date"), JsonProperty("date")]
    public DateTime Date { get; set; }

    [Display("Description"), JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [Display("Due date"), JsonProperty("due_date")]
    public DateTime? DueDate { get; set; }

    [Display("Invoice ID"), JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [Display("Items"), JsonProperty("items")]
    public List<InvoiceItemResponse> Items { get; set; } = new();
    
    [Display("Item amounts")]
    public List<double> ItemAmounts { get; set; } = new ();
    
    [Display("Item descriptions")]
    public List<string> ItemDescriptions { get; set; } = new ();

    [Display("Invoice Number"), JsonProperty("number")]
    public string Number { get; set; } = string.Empty;

    [Display("Paid Out At"), JsonProperty("paid_out_at")]
    public DateTime? PaidOutAt { get; set; }

    [Display("Source Amount"), JsonProperty("source_amount")]
    public double SourceAmount { get; set; }

    [Display("Source Currency"), JsonProperty("source_currency")]
    public string SourceCurrency { get; set; } = string.Empty;

    [Display("Status"), JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [Display("Target Amount"), JsonProperty("target_amount")]
    public double? TargetAmount { get; set; }

    [Display("Target Currency"), JsonProperty("target_currency")]
    public string TargetCurrency { get; set; } = string.Empty;
    
    public void SetItemAmountsAndDescriptions()
    {
        ItemAmounts = Items.Select(x => (double)x.Amount).ToList();
        ItemDescriptions = Items.Select(x => x.Description).ToList();
    }
}