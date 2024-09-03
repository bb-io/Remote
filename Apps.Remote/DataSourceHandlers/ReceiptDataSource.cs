using Apps.Remote.Actions;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Identifiers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.DataSourceHandlers;

public class ReceiptDataSource(InvocationContext invocationContext, [ActionParameter] ExpenseIdentifier identifier)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(identifier.ExpenseId))
        {
            throw new InvalidOperationException("You should provide an Expense ID first.");
        }
        
        var expenseActions = new ExpenseActions(InvocationContext, null!);
        var expense = await expenseActions.GetExpense(identifier);

        return expense.Receipts?
                   .Where(x => context.SearchString == null || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Id, x => x.Name)
               ?? new Dictionary<string, string>();
    }
}