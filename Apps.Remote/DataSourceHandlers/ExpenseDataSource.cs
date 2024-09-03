using Apps.Remote.Actions;
using Apps.Remote.Invocables;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Remote.DataSourceHandlers;

public class ExpenseDataSource(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var expenseActions = new ExpenseActions(InvocationContext, null!);
        var expensesResponse = await expenseActions.GetAllExpenses();

        return expensesResponse.Expenses?
                   .Where(x => context.SearchString == null || x.Title.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                   .ToDictionary(x => x.Id, x => x.Title)
               ?? new Dictionary<string, string>();
    }
}