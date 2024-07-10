using Apps.Remote.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.InvoiceSchedules;

public class InvoiceSchedulesResponse : BasePaginationResponse
{
    [Display("Invoice schedules"), JsonProperty("contractor_invoice_schedules")]
    public List<InvoiceScheduleResponse>? InvoiceSchedules { get; set; } = new();
}