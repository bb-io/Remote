using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.InvoiceSchedules;

public class CreatedInvoiceScheduleDto
{
    [Display("Invoice schedules"), JsonProperty("successes")]
    public List<InvoiceScheduleResponse> InvoiceScheduleResponses { get; set; } = new();
}