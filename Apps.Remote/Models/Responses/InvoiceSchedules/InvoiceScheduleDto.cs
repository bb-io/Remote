using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.InvoiceSchedules;

public class InvoiceScheduleDto
{
    [JsonProperty("contractor_invoice_schedule")]
    public InvoiceScheduleResponse InvoiceSchedule { get; set; } = new();
}