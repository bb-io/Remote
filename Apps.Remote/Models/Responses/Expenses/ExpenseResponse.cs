using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses.Expenses;

public class ExpenseResponse
{
    [Display("Amount")]
    public int Amount { get; set; }

    [Display("Currency")]
    public CurrencyResponse Currency { get; set; }

    [Display("Employment ID")]
    public string EmploymentId { get; set; }

    [Display("Expense date")]
    public DateTime ExpenseDate { get; set; }

    [Display("Receipts")]
    public IEnumerable<ReceiptResponse> Receipts { get; set; }

    [Display("Tax amount")]
    public int TaxAmount { get; set; }

    [Display("Title")]
    public string Title { get; set; }
}