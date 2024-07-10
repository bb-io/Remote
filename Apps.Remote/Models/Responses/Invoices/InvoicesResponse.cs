using Apps.Remote.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.Invoices;

public class InvoicesResponse : BasePaginationResponse
{
    [Display("Invoice schedules"), JsonProperty("contractor_invoices")]
    public List<InvoiceResponse>? Invoices { get; set; } = new();
}