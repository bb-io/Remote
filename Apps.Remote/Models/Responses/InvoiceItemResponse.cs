using Newtonsoft.Json;

namespace Apps.Remote.Models.Responses;

public class InvoiceItemResponse
{
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [JsonProperty("amount")]
    public decimal Amount { get; set; }
}