using Apps.Remote.Api;
using Apps.Remote.Constants;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Requests.Expenses;
using Apps.Remote.Models.Responses.Expenses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Remote.Actions;

[ActionList]
public class ExpenseActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Create expense", Description = "Create a new expense")]
    public async Task<ExpenseResponse> CreateExpense([ActionParameter] CreateExpenseRequest input)
    {
        var apiRequest = new ApiRequest("/v1/expenses", Method.Post, Creds)
            .WithJsonBody(input, JsonConfig.JsonSettings);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<ExpenseDto>>(apiRequest);

        return response.Data?.Expense!;
    }
}