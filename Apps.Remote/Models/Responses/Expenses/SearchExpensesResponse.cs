using Apps.Remote.Models.Dtos;

namespace Apps.Remote.Models.Responses.Expenses;

public class SearchExpensesResponse : BasePaginationResponse
{
    public List<ExpenseResponse>? Expenses { get; set; }
}