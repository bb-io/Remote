using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses.Invoices;

public class InvoiceDto
{
    [JsonProperty("contractor_invoice")]
    public InvoiceResponse Invoice { get; set; } = new();
}