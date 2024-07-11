using System.Globalization;
using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.InvoiceSchedules;
using Apps.Remote.Models.Responses.InvoiceSchedules;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Remote.Actions;

[ActionList]
public class InvoiceSchedulesActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    private const int DefaultPageSize = 50;

    [Action("Search invoice schedules", Description = "Search invoice schedules based on specified criteria")]
    public async Task<InvoiceSchedulesResponse> SearchInvoiceSchedules([ActionParameter] SearchInvoiceSchedulesRequest request)
    {
        var allInvoiceSchedules = new List<InvoiceScheduleResponse>();
        var currentPage = 1;
        InvoiceSchedulesResponse invoiceSchedulesResponse;

        do
        {
            var apiRequest = CreateApiRequest(request, currentPage, DefaultPageSize);
            var response = await Client.ExecuteWithErrorHandling<BaseDto<InvoiceSchedulesResponse>>(apiRequest);
            invoiceSchedulesResponse = response.Data ?? new InvoiceSchedulesResponse();

            if (invoiceSchedulesResponse.InvoiceSchedules != null)
            {
                invoiceSchedulesResponse.InvoiceSchedules.ForEach(x => x.SetItemAmountsAndDescriptions());
                allInvoiceSchedules.AddRange(invoiceSchedulesResponse.InvoiceSchedules);
            }

            currentPage++;
        } while (currentPage <= invoiceSchedulesResponse.TotalPages);

        return new InvoiceSchedulesResponse
        {
            TotalCount = allInvoiceSchedules.Count,
            CurrentPage = 1,
            TotalPages = 1,
            InvoiceSchedules = allInvoiceSchedules
        };
    }
    
    [Action("Get invoice schedule", Description = "Get invoice schedule by ID")]
    public async Task<InvoiceScheduleResponse> GetInvoiceSchedule([ActionParameter] InvoiceScheduleIdentifier identifier)
    {
        var apiRequest = new ApiRequest($"/v1/contractor-invoice-schedules/{identifier.InvoiceScheduleId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<InvoiceScheduleDto>>(apiRequest);
        
        response.Data!.InvoiceSchedule.SetItemAmountsAndDescriptions();
        return response.Data!.InvoiceSchedule;
    }
    
    [Action("Create invoice schedule", Description = "Create invoice schedule with specified data.")]
    public async Task<InvoiceScheduleResponse> CreateInvoiceSchedule([ActionParameter] CreateInvoiceScheduleRequest request)
    {
        if(request.Amounts.Count != request.Descriptions.Count)
            throw new ArgumentException("Amounts and Descriptions must have the same number of elements");
        
        var body = new
        {
            contractor_invoice_schedules = new[]
            {
                new
                {
                    employment_id = request.EmploymentId,
                    currency = request.Currency,
                    start_date = request.StartDate.ToString("yyyy-MM-dd"),
                    periodicity = request.Periodicity,
                    items = request.Amounts.Select((amount, index) => new
                    {
                        amount = amount.ToString(CultureInfo.InvariantCulture),
                        description = request.Descriptions.ElementAtOrDefault(index)
                    }),
                    note = request.Note ?? string.Empty,
                    nr_occurrences = request.NrOccurrences.HasValue ? request.NrOccurrences.Value.ToString() : "1",
                    number = request.Number ?? "1"
                }
            }
        };
        
        var apiRequest = new ApiRequest("/v1/contractor-invoice-schedules", Method.Post, Creds)
            .WithJsonBody(body);
        
        var response = await Client.ExecuteWithErrorHandling<BaseDto<InvoiceScheduleDto>>(apiRequest);
        response.Data!.InvoiceSchedule.SetItemAmountsAndDescriptions();
        return response.Data!.InvoiceSchedule;
    }
    
    [Action("Update invoice schedule", Description = "Update invoice schedule by ID with specified data")]
    public async Task<InvoiceScheduleResponse> UpdateInvoiceSchedule([ActionParameter] UpdateInvoiceScheduleRequest request)
    {
        if (request.Amounts?.Count != request.Descriptions?.Count)
        {
            throw new ArgumentException("Amounts and Descriptions must have the same number of elements");
        }
        
        var items = request.Amounts?.Select((amount, index) => new
        {
            amount = amount.ToString(CultureInfo.InvariantCulture),
            description = request.Descriptions!.ElementAtOrDefault(index)
        }).ToList();
        
        var body = new
        {
            currency = request.Currency,
            periodicity = request.Periodicity,
            items = items,
            note = request.Note,
            nr_occurrences = request.NrOccurrences,
            number = request.Number
        };
        
        var apiRequest = new ApiRequest($"/v1/contractor-invoice-schedules/{request.InvoiceScheduleId}", Method.Patch, Creds)
            .WithJsonBody(body);
        
        var response = await Client.ExecuteWithErrorHandling<BaseDto<InvoiceScheduleDto>>(apiRequest);
        response.Data!.InvoiceSchedule.SetItemAmountsAndDescriptions();
        return response.Data!.InvoiceSchedule;
    }

    private ApiRequest CreateApiRequest(SearchInvoiceSchedulesRequest request, int currentPage, int pageSize)
    {
        var apiRequest = new ApiRequest("/v1/contractor-invoice-schedules", Method.Get, Creds);

        if (request.StartDateFrom.HasValue)
        {
            apiRequest.AddParameter("start_date_from", request.StartDateFrom.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.StartDateTo.HasValue)
        {
            apiRequest.AddParameter("start_date_to", request.StartDateTo.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.NextInvoiceDateFrom.HasValue)
        {
            apiRequest.AddParameter("next_invoice_date_from", request.NextInvoiceDateFrom.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.NextInvoiceDateTo.HasValue)
        {
            apiRequest.AddParameter("next_invoice_date_to", request.NextInvoiceDateTo.Value.ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            apiRequest.AddParameter("status", request.Status, ParameterType.QueryString);
        }

        if (!string.IsNullOrEmpty(request.EmploymentId))
        {
            apiRequest.AddParameter("employment_id", request.EmploymentId, ParameterType.QueryString);
        }

        if (!string.IsNullOrEmpty(request.Periodicity))
        {
            apiRequest.AddParameter("periodicity", request.Periodicity, ParameterType.QueryString);
        }

        if (!string.IsNullOrEmpty(request.Currency))
        {
            apiRequest.AddParameter("currency", request.Currency, ParameterType.QueryString);
        }

        apiRequest.AddParameter("page", currentPage, ParameterType.QueryString);
        apiRequest.AddParameter("page_size", pageSize, ParameterType.QueryString);

        return apiRequest;
    }
}