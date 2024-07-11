using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Requests.Invoices;
using Apps.Remote.Models.Responses.Invoices;
using Apps.Remote.Polling.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Remote.Polling;

[PollingEventList]
public class PollingList(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    private const int DefaultPageSize = 50;
    
    [PollingEvent("On invoices approved",
        Description = "Returns approved invoices that was approved after the last polling time")]
    public async Task<PollingEventResponse<PageMemory, InvoicesResponse>> OnInvoicesApproved(
        PollingEventRequest<PageMemory> request)
    {
        if (request.Memory is null || request.Memory.LastPollingTime is null)
        {
            return new PollingEventResponse<PageMemory, InvoicesResponse>
            {
                FlyBird = false,
                Memory = new PageMemory { LastPollingTime = DateTime.UtcNow },
                Result = null
            };
        }

        var approvedBlogPosts = await SearchInvoices(new SearchInvoicesRequest { ApprovedDateFrom = request.Memory.LastPollingTime });

        if (approvedBlogPosts.Invoices?.Count == 0)
        {
            return new PollingEventResponse<PageMemory, InvoicesResponse>
            {
                FlyBird = false,
                Memory = new PageMemory { LastPollingTime = DateTime.UtcNow },
                Result = null
            };
        }

        return new PollingEventResponse<PageMemory, InvoicesResponse>
        {
            FlyBird = true,
            Memory = new PageMemory { LastPollingTime = DateTime.UtcNow },
            Result = approvedBlogPosts
        };
    }

    private async Task<InvoicesResponse> SearchInvoices([ActionParameter] SearchInvoicesRequest request)
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

        return new InvoicesResponse
        {
            TotalCount = allInvoices.Count,
            CurrentPage = 1,
            TotalPages = 1,
            Invoices = allInvoices
        };
    }

    private ApiRequest CreateApiRequest(SearchInvoicesRequest request, int currentPage, int pageSize)
    {
        var apiRequest = new ApiRequest("/v1/contractor-invoices", Method.Get, Creds);

        if (!string.IsNullOrEmpty(request.Status))
        {
            apiRequest.AddParameter("status", request.Status, ParameterType.QueryString);
        }

        if (!string.IsNullOrEmpty(request.ContractorInvoiceScheduleId))
        {
            apiRequest.AddParameter("contractor_invoice_schedule_id", request.ContractorInvoiceScheduleId, ParameterType.QueryString);
        }

        if (request.DateFrom.HasValue)
        {
            apiRequest.AddParameter("date_from", request.DateFrom.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.DateTo.HasValue)
        {
            apiRequest.AddParameter("date_to", request.DateTo.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.DueDateFrom.HasValue)
        {
            apiRequest.AddParameter("due_date_from", request.DueDateFrom.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.DueDateTo.HasValue)
        {
            apiRequest.AddParameter("due_date_to", request.DueDateTo.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.ApprovedDateFrom.HasValue)
        {
            apiRequest.AddParameter("approved_date_from", request.ApprovedDateFrom.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.ApprovedDateTo.HasValue)
        {
            apiRequest.AddParameter("approved_date_to", request.ApprovedDateTo.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.PaidOutDateFrom.HasValue)
        {
            apiRequest.AddParameter("paid_out_date_from", request.PaidOutDateFrom.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.PaidOutDateTo.HasValue)
        {
            apiRequest.AddParameter("paid_out_date_to", request.PaidOutDateTo.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        apiRequest.AddParameter("page", currentPage, ParameterType.QueryString);
        apiRequest.AddParameter("page_size", pageSize, ParameterType.QueryString);

        return apiRequest;
    }
}