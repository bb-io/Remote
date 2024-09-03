using Apps.Remote.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Remote.Models.Identifiers;

public class ExpenseIdentifier
{
    [Display("Expense ID"), DataSource(typeof(ExpenseDataSource))]
    public string ExpenseId { get; set; } = string.Empty;
}