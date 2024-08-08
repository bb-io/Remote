using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Requests.Invoices;
using Apps.Remote.Models.Responses.Invoices;
using Apps.Remote.Polling.Models;
using Apps.Remote.Polling.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Remote.Polling;

[PollingEventList]
public class PollingList(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    private const int DefaultPageSize = 50;
    
    [PollingEvent("On invoices status changed",
        Description = "Returns invoices that changed status after the last polling time")]
    public async Task<PollingEventResponse<PageMemory, InvoicesResponse>> OnInvoicesStatusChanged(
        PollingEventRequest<PageMemory> request,
        [PollingEventParameter] OnInvoicesStatusChangedRequest statusChangedRequest)
    {
        try
        {
            if (request.Memory is null)
            {
                return new PollingEventResponse<PageMemory, InvoicesResponse>
                {
                    FlyBird = false,
                    Memory = new PageMemory(),
                    Result = null
                };
            }

            var invoices = await SearchInvoices(new SearchInvoicesRequest { Status = statusChangedRequest.Status });
            var memories = request.Memory.PageMemoryDtos;
            var changedInvoices = invoices.Invoices!
                .Where(x => memories.All(m => m.Id != x.Id))
                .ToList();
            await Logger.LogAsync(new { changedInvoices, invoices, memories });

            if (changedInvoices.Count == 0)
            {
                return new PollingEventResponse<PageMemory, InvoicesResponse>
                {
                    FlyBird = false,
                    Memory = request.Memory,
                    Result = null
                };
            }
        
            memories.AddRange(changedInvoices.Select(x => new PageMemoryDto { Id = x.Id, Status = x.Status }));
            await Logger.LogAsync(new { memories });

            return new PollingEventResponse<PageMemory, InvoicesResponse>
            {
                FlyBird = true,
                Memory = new PageMemory { PageMemoryDtos = memories },
                Result = new InvoicesResponse
                {
                    TotalCount = changedInvoices.Count,
                    CurrentPage = 1,
                    TotalPages = 1,
                    Invoices = changedInvoices
                }
            };
            
            /*var invoices = await SearchInvoices(new SearchInvoicesRequest { Status = statusChangedRequest.Status });
            var memories = request.Memory?.PageMemoryDtos ?? new List<PageMemoryDto>();
            var changedInvoices = invoices.Invoices!
                .Where(x => memories.All(m => m.Id != x.Id))
                .ToList();
            
            await Logger.LogAsync(new { changedInvoices, invoices, memories });
            if (request.Memory is null)
            {
                return new PollingEventResponse<PageMemory, InvoicesResponse>
                {
                    FlyBird = false,
                    Memory = new PageMemory
                    {
                        PageMemoryDtos = changedInvoices.Select(x => new PageMemoryDto { Id = x.Id, Status = x.Status })
                            .ToList()
                    },
                    Result = null
                };
            }

            if (changedInvoices.Count == 0)
            {
                return new PollingEventResponse<PageMemory, InvoicesResponse>
                {
                    FlyBird = false,
                    Memory = request.Memory,
                    Result = null
                };
            }

            memories.AddRange(changedInvoices.Select(x => new PageMemoryDto { Id = x.Id, Status = x.Status }));
            await Logger.LogAsync(new { memories });
            return new PollingEventResponse<PageMemory, InvoicesResponse>
            {
                FlyBird = true,
                Memory = new PageMemory { PageMemoryDtos = memories },
                Result = new InvoicesResponse
                {
                    TotalCount = changedInvoices.Count,
                    CurrentPage = 1,
                    TotalPages = 1,
                    Invoices = changedInvoices
                }
            };*/
        }
        catch (Exception e)
        {
            await Logger.LogAsync(e);
            throw;
        }
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
                invoiceResponse.Invoices.ForEach(x => x.SetItemAmountsAndDescriptions());
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