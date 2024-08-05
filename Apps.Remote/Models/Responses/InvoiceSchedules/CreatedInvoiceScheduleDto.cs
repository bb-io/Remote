using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System.Text;

namespace Apps.Remote.Models.Responses.InvoiceSchedules;

public class CreatedInvoiceScheduleDto
{
    [Display("Invoice schedules"), JsonProperty("successes")]
    public List<InvoiceScheduleResponse> InvoiceScheduleResponses { get; set; } = new();

    [JsonProperty("failures")]
    public List<InvoiceScheduleErrors> InvoiceScheduleErrors { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var invoiceScheduleError in InvoiceScheduleErrors)
        {
            sb.Append(invoiceScheduleError.ToString());
        }

        return sb.ToString();
    }
}

public class InvoiceScheduleErrors
{
    [JsonProperty("errors")]
    public Dictionary<string, List<string>> Errors { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var (key, value) in Errors)
        {
            sb.Append($"{key}: {string.Join(", ", value)}; ");
        }

        return sb.ToString();
    }
}