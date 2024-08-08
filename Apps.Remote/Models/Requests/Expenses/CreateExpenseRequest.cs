namespace Apps.Remote.Models.Requests.Expenses;

public class CreateExpenseRequest
{
    public string Title { get; set; }

    public int Amount { get; set; }

    public string Currency { get; set; }

    public string EmploymentId { get; set; }

    public string ExpenseDate { get; set; }

    public ReceiptRequest Receipt { get; set; }

    public CreateExpenseRequest(CreateExpenseInput input, byte[] fileBytes)
    {
        Title = input.Title;
        Amount = input.Amount;
        Currency = input.Currency;
        EmploymentId = input.EmploymentId;
        ExpenseDate = input.ExpenseDate.ToString("yyyy-MM-dd");
        Receipt = new()
        {
            Name = input.ReceiptFile.Name,
            Content = Convert.ToBase64String(fileBytes)
        };
    }
}