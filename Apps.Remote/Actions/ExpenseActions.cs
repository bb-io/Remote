using Apps.Remote.Api;
using Apps.Remote.Constants;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.Expenses;
using Apps.Remote.Models.Responses.Expenses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Remote.Actions;

[ActionList]
public class ExpenseActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : AppInvocable(invocationContext)
{
    private const int PageSize = 50;
    
    [Action("Get expenses", Description = "Get all expenses")]
    public async Task<SearchExpensesResponse> GetAllExpenses()
    {
        var allExpenses = new List<ExpenseResponse>();
        var currentPage = 1;
        SearchExpensesResponse expensesResponse;

        do
        {
            var apiRequest = CreateApiRequest(currentPage, PageSize);
            var response = await Client.ExecuteWithErrorHandling<BaseDto<SearchExpensesResponse>>(apiRequest);
            expensesResponse = response.Data ?? new SearchExpensesResponse();

            if (expensesResponse.Expenses != null)
            {
                allExpenses.AddRange(expensesResponse.Expenses);
            }

            currentPage++;
        } while (currentPage <= expensesResponse.TotalPages);

        return new SearchExpensesResponse
        {
            TotalCount = allExpenses.Count,
            CurrentPage = 1,
            TotalPages = 1,
            Expenses = allExpenses
        };
    }
    
    [Action("Get expense", Description = "Get a single expense")]
    public async Task<ExpenseResponse> GetExpense([ActionParameter] ExpenseIdentifier identifier)
    {
        var apiRequest = new ApiRequest($"/v1/expenses/{identifier.ExpenseId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<ExpenseDto>>(apiRequest);
        return response.Data?.Expense!;
    }
    
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
    
    [Action("Download receipt", Description = "Download a receipt file for an expense")]
    public async Task<FileReference> DownloadReceipt([ActionParameter] ReceiptIdentifier identifier)
    {
        var expense = await GetExpense(identifier);
        var receipt = expense.Receipts.FirstOrDefault(r => r.Id == identifier.ReceiptId)
            ?? throw new PluginMisconfigurationException("Receipt not found in the expense object.");
        
        var apiRequest = new ApiRequest($"/v1/expenses/{identifier.ExpenseId}/receipts/{identifier.ReceiptId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling(apiRequest);
        var memoryStream = new MemoryStream(response.RawBytes!);
        memoryStream.Position = 0;

        return await fileManagementClient.UploadAsync(memoryStream, MimeTypes.GetMimeType(receipt.Name), receipt.Name);
    }
    
    private ApiRequest CreateApiRequest(int currentPage, int pageSize)
    {
        var apiRequest = new ApiRequest("/v1/expenses", Method.Get, Creds);

        apiRequest.AddParameter("page", currentPage, ParameterType.QueryString);
        apiRequest.AddParameter("page_size", pageSize, ParameterType.QueryString);

        return apiRequest;
    }
}