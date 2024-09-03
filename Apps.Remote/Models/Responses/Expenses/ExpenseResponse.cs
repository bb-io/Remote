using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses.Expenses;

public class ExpenseResponse
{
    [Display("Expense ID")]
    public string Id { get; set; } = string.Empty;
    
    [Display("Amount")]
    public double Amount { get; set; }

    [Display("Currency")] 
    public CurrencyResponse Currency { get; set; } = new();

    [Display("Employment ID")]
    public string EmploymentId { get; set; } = string.Empty;

    [Display("Expense date")]
    public DateTime ExpenseDate { get; set; }

    [Display("Receipts")]
    public List<ReceiptResponse> Receipts { get; set; } = new();

    [Display("Tax amount")]
    public int TaxAmount { get; set; }

    [Display("Title")]
    public string Title { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}