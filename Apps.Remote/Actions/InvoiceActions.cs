using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.Invoices;
using Apps.Remote.Models.Responses.Invoices;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Remote.Actions;

[ActionList]
public class InvoiceActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    private const int DefaultPageSize = 50;
    
    [Action("Search invoices", Description = "Search invoices based on specified criteria")]
    public async Task<InvoicesResponse> SearchInvoices([ActionParameter] SearchInvoicesRequest request)
    {
        var allInvoices = new List<InvoiceResponse>();
        var currentPage = 1;
        InvoicesResponse invoiceResponse;

        do
        {
            var apiRequest = CreateApiRequest(request, currentPage, DefaultPageSize);
            var response = await Client.ExecuteWithErrorHandling<BaseDto<InvoicesResponse>>(apiRequest);
            invoiceResponse = response.Data ?? new InvoicesResponse();

            if (invoiceResponse.Invoices != null)
            {
                allInvoices.AddRange(invoiceResponse.Invoices);
            }

            currentPage++;
        } while (currentPage <= invoiceResponse.TotalPages);
        
        if(request.EmploymentId != null)
        {
            // Since the API does not support filtering by Employment ID, we need to filter the results manually (https://developer.remote.com/reference/get_index_contractor_invoice-1)
            allInvoices = allInvoices.Where(i => i.EmploymentId == request.EmploymentId).ToList();
        }

        return new InvoicesResponse
        {
            TotalCount = allInvoices.Count,
            CurrentPage = 1,
            TotalPages = 1,
            Invoices = allInvoices
        };
    }
    
    [Action("Get invoice", Description = "Get invoice by ID")]
    public async Task<InvoiceResponse> GetInvoice([ActionParameter] InvoiceIdentifier identifier)
    {
        var apiRequest = new ApiRequest($"/v1/contractor-invoices/{identifier.InvoiceId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<InvoiceDto>>(apiRequest);

        response.Data?.Invoice.SetItemAmountsAndDescriptions();
        return response.Data!.Invoice;
    }
    
    private ApiRequest CreateApiRequest(SearchInvoicesRequest request, int currentPage, int pageSize)
    {
        var apiRequest = new ApiRequest("/v1/contractor-invoices", Method.Get, Creds);

        if (!string.IsNullOrEmpty(request.Status))
        {
            apiRequest.AddParameter("status", request.Status);
        }

        if (!string.IsNullOrEmpty(request.ContractorInvoiceScheduleId))
        {
            apiRequest.AddParameter("contractor_invoice_schedule_id", request.ContractorInvoiceScheduleId);
        }

        if (request.DateFrom.HasValue)
        {
            apiRequest.AddParameter("date_from", request.DateFrom.Value.ToString("yyyy-MM-dd"));
        }

        if (request.DateTo.HasValue)
        {
            apiRequest.AddParameter("date_to", request.DateTo.Value.ToString("yyyy-MM-dd"));
        }

        if (request.DueDateFrom.HasValue)
        {
            apiRequest.AddParameter("due_date_from", request.DueDateFrom.Value.ToString("yyyy-MM-dd"));
        }

        if (request.DueDateTo.HasValue)
        {
            apiRequest.AddParameter("due_date_to", request.DueDateTo.Value.ToString("yyyy-MM-dd"));
        }

        if (request.ApprovedDateFrom.HasValue)
        {
            apiRequest.AddParameter("approved_date_from", request.ApprovedDateFrom.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.ApprovedDateTo.HasValue)
        {
            apiRequest.AddParameter("approved_date_to", request.ApprovedDateTo.Value.ToString("yyyy-MM-dd"));
        }

        if (request.PaidOutDateFrom.HasValue)
        {
            apiRequest.AddParameter("paid_out_date_from", request.PaidOutDateFrom.Value.ToString("yyyy-MM-dd"));
        }

        if (request.PaidOutDateTo.HasValue)
        {
            apiRequest.AddParameter("paid_out_date_to", request.PaidOutDateTo.Value.ToString("yyyy-MM-dd"));
        }
        
        apiRequest.AddParameter("page", currentPage);
        apiRequest.AddParameter("page_size", pageSize);

        return apiRequest;
    }
}