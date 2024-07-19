using System.Globalization;
using System.Text;
using Apps.Remote.Api;
using Apps.Remote.Invocables;
using Apps.Remote.Models.Dtos;
using Apps.Remote.Models.Identifiers;
using Apps.Remote.Models.Requests.InvoiceSchedules;
using Apps.Remote.Models.Responses.InvoiceSchedules;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Remote.Actions;

[ActionList]
public class InvoiceSchedulesActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : AppInvocable(invocationContext)
{
    private const int DefaultPageSize = 50;

    [Action("Search invoice schedules", Description = "Search сontractor invoice schedules based on specified criteria")]
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

        if (request.Number != null)
        {
            allInvoiceSchedules = allInvoiceSchedules.Where(x => x.Number == request.Number).ToList();
        }

        return new InvoiceSchedulesResponse
        {
            TotalCount = allInvoiceSchedules.Count,
            CurrentPage = 1,
            TotalPages = 1,
            InvoiceSchedules = allInvoiceSchedules
        };
    }
    
    [Action("Get invoice schedule", Description = "Get сontractor invoice schedule by ID")]
    public async Task<InvoiceScheduleResponse> GetInvoiceSchedule([ActionParameter] InvoiceScheduleIdentifier identifier)
    {
        var apiRequest = new ApiRequest($"/v1/contractor-invoice-schedules/{identifier.InvoiceScheduleId}", Method.Get, Creds);
        var response = await Client.ExecuteWithErrorHandling<BaseDto<InvoiceScheduleDto>>(apiRequest);
        
        response.Data!.InvoiceSchedule.SetItemAmountsAndDescriptions();
        return response.Data!.InvoiceSchedule;
    }
    
    [Action("Create invoice schedule", Description = "Create сontractor invoice schedule with specified data.")]
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
                    start_date = request.StartDate.ToUniversalTime().ToString("yyyy-MM-dd"),
                    periodicity = request.Periodicity,
                    items = request.Amounts.Select((amount, index) => new
                    {
                        amount = (int)amount,
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
        
        var invoiceScheduleResponses = await Client.ExecuteWithErrorHandling<BaseDto<CreatedInvoiceScheduleDto>>(apiRequest);
        
        var invoice = invoiceScheduleResponses.Data!.InvoiceScheduleResponses.FirstOrDefault() ?? throw new Exception("Invoice schedule returned unexpected response.");
        invoice.SetItemAmountsAndDescriptions();
        return invoice;
    }
    
    [Action("Import invoice schedules", Description = "Import сontractor invoice schedules from a JSON file")]
    public async Task<InvoiceScheduleResponse> ImportInvoiceSchedules([ActionParameter] ImportInvoiceRequest request)
    {
        var stream = await fileManagementClient.DownloadAsync(request.File);
        var bytes = await stream.GetByteData();
        var json = Encoding.UTF8.GetString(bytes);
        
        var invoicesDto = JsonConvert.DeserializeObject<BlackbirdInvoiceDto>(json)!;
        var invoiceToImport = invoicesDto.Invoices.First();

        var result = int.TryParse(invoiceToImport.InvoiceNumber, out var number);
        var apiNumber = request.Number ?? (result ? number.ToString() : "1");
        var startDate = request.StartDate ?? (invoiceToImport.InvoiceDate < DateTime.Now ? DateTime.Now.AddDays(7) : invoiceToImport.InvoiceDate);
        
        var amounts = invoiceToImport.Lines.Select(line => line.Amount).ToList();
        var sum = amounts.Sum();
        if (sum < 100)
        {
            throw new ArgumentException("Sum of amounts must be greater than 100");
        }
        
        var createRequest = new CreateInvoiceScheduleRequest
        {
            EmploymentId = request.EmploymentId,
            Currency = invoiceToImport.Currency,
            StartDate = startDate,
            Periodicity = request.Periodicity ?? "monthly",
            Amounts = amounts,
            Descriptions = invoiceToImport.Lines.Select(line => line.Description).ToList(),
            Note = request.Description ?? $"Invoice imported from external system. Original invoice number: {invoiceToImport.InvoiceNumber}",
            NrOccurrences = request.NrOccurrences ?? 1,
            Number = apiNumber
        };

        return await CreateInvoiceSchedule(createRequest);
    }
    
    [Action("Update invoice schedule", Description = "Update сontractor invoice schedule by ID with specified data")]
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
            apiRequest.AddParameter("start_date_to", request.StartDateTo.Value.ToUniversalTime().ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.NextInvoiceDateFrom.HasValue)
        {
            apiRequest.AddParameter("next_invoice_date_from", request.NextInvoiceDateFrom.Value.ToUniversalTime().ToString("yyyy-MM-dd"), ParameterType.QueryString);
        }

        if (request.NextInvoiceDateTo.HasValue)
        {
            apiRequest.AddParameter("next_invoice_date_to", request.NextInvoiceDateTo.Value.ToUniversalTime().ToString("yyyy-MM-dd"), ParameterType.QueryString);
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