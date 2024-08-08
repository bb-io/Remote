using Apps.Remote.Api;
using Apps.Remote.Constants;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Requests.Expenses;
using Apps.Remote.Models.Responses.Expenses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Remote.Actions;

[ActionList]
public class ExpenseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : AppInvocable(invocationContext)
{
    [Action("Create expense", Description = "Create a new expense")]
    public async Task<ExpenseResponse> CreateExpense([ActionParameter] CreateExpenseInput input)
    {
        var fileStream = await fileManagementClient.DownloadAsync(input.ReceiptFile);
        var fileBytes = await fileStream.GetByteData();
        
        var apiRequest = new ApiRequest("/v1/expenses", Method.Post, Creds)
            .WithJsonBody(new CreateExpenseRequest(input, fileBytes), JsonConfig.JsonSettings);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<ExpenseDto>>(apiRequest);

        return response.Data?.Expense!;
    }
}