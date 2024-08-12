using Apps.Remote.DataSourceHandlers;
using Apps.Remote.DataSourceHandlers.Static;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Remote.Models.Requests.Expenses;

public class CreateExpenseInput
{
    public string Title { get; set; }

    public int Amount { get; set; }

    [Display("Currency", Description = "The three-letter code for the expense currency."), StaticDataSource(typeof(CurrencyDataSource))]
    public string Currency { get; set; }

    [Display("Employment ID"), DataSource(typeof(EmploymentDataSource))]
    public string EmploymentId { get; set; }

    [Display("Expense date")] public DateTime ExpenseDate { get; set; }
    
    [Display("Receipt file")] public FileReference ReceiptFile { get; set; }
}